using Amazon;
using Amazon.Configurations;
using Amazon.Runtime;
using Amazon.S3.FileUploadService;
using Amazon.Secrets;
using AWS.Logger;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Poliedro.Eds.Api;
using Poliedro.Eds.Api.Common.Configurations;
using Poliedro.Eds.Api.Middlelware.aws;
using Poliedro.Eds.Api.Middlelware.Jwt;
using Poliedro.Eds.Application;
using Poliedro.Eds.Application.AWS.Configurations.Dto.Plemsi;
using Poliedro.Eds.Application.Court.Queris.GetCourtList;
using Poliedro.Eds.Application.FileUploadS3.Command;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Secrets.Aws.Dto;
using Poliedro.Eds.Application.Translations.Dtos;
using Poliedro.Eds.Application.Translations.Handle;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.FileUploadS3.Ports;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;
using Poliedro.Eds.Infraestructure.External.Plemsi;
using Poliedro.Eds.Infraestructure.Persistence.Mysql;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Inventory.Repositories;
using Poliedro.External.HealthCheck.Tolgee;
using Poliedro.Tolgee;
using Poliedro.Tolgee.Translations;
using System.Net.Http.Headers;
using WorkerKeycloackService;

var builder = WebApplication.CreateBuilder(args);


// Configura el logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Configuration
    .AddSecretsManager("poliedro-conecctionstring-mysql-eds-backend", "us-east-2");
builder.Services
    .AddWebApi()
    .AddApplication()
    .AddExternalPlemsi(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddExternalAmazon()
    .AddExternalTolgee();

builder.Services.AddHostedService<Worker>();
builder.Services.AddHealthChecks()
    .AddMySql(builder.Configuration.GetConnectionString("MysqlConnection"), name: "sql", tags: ["ready"])
    .AddRedis(builder.Configuration["Redis:ConnectionString"], name: "redis", tags: ["ready"])
    .AddCheck<TolgeeHealthCheckService>("Service Health Check Tolgee"); 

builder.Services.AddLogging();


builder.Services.AddHttpClient<IKeycloakUserService, KeycloakService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Keycloak:KeycloakUri"]);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});


builder.Services.AddSingleton<RabbitMQ.Client.IConnection>(sp =>
{
    var factory = new RabbitMQ.Client.ConnectionFactory()
    {
        HostName = builder.Configuration["RabbitMQ:HostName"],
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"]
    };
    return factory.CreateConnection();
});

// Configura el JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Auth"];
        options.Audience = builder.Configuration["Keycloak:ClientId"];
        options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,

            ClockSkew = TimeSpan.Zero
        };
    });

// Configura politica
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole( "Admin"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrIslander", policy =>
    policy.RequireRole("Admin", "User"));

});

// Configura el AddSwaggerWithJwt
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });

    options.CustomSchemaIds(type => type.FullName);
});


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetTranslationsHandler>();
    cfg.RegisterServicesFromAssemblyContaining<GetCourtsListQueryHandler>();
});

builder.Services.AddMemoryCache();
builder.Services.Configure<TolgeeSettings>(builder.Configuration.GetSection("Tolgee"));
builder.Services.AddHttpClient<TranslationCachingService>((serviceProvider, client) =>
{
    var apiSettings = builder.Configuration.GetSection("Tolgee").Get<TolgeeSettings>();
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("X-API-Key", apiSettings.ApiKey);
    client.Timeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddHostedService<TranslationCachingService>();
builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection("Redis"));
builder.Services.AddTransient<IFileUploadService, FileUploadService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadFileCommand).Assembly));
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionConfiguration>();
});

builder.Services.AddRouting(routing => routing.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IGetExpenditureId, GetIdAuxService>();
builder.Services.AddScoped<IGetTypeOfCollectionId, GetIdAuxService>();
builder.Services.AddScoped<IGetProductAndCompartiment, GetIdAuxService>();
builder.Services.AddScoped<IProductCompartimentGetByCompartmentId, ProductCompartimentGetByCompartmentId>();

builder.Services.AddScoped<ICourtListDomainService, CourtListService>();
builder.Services.AddScoped<IInventoryListDomainService, InventoryListService>();

builder.Services.AddValidatorsFromAssemblyContaining<GetCourtsListQueryValidator>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});
AwsSecretsDto secret = await AwsSecrets.GetSecret(builder.Configuration);


var loggerConfig = new AWSLoggerConfig
{
    Region = secret.Region,
    Credentials = new BasicAWSCredentials(secret.AwsAccessKeyId, secret.AwsSecretAccessKey),
    LogGroup = "poliedro-eds-group",

};

builder.Logging.ClearProviders();
builder.Logging.AddAWSProvider(loggerConfig);
builder.Logging.SetMinimumLevel(LogLevel.Information);


var connectionString = builder.Configuration.GetConnectionString("MysqlConnection");

var appConfigService = new AwsAppConfigService("hpgip50", "t3iui3q", "wfcp470", RegionEndpoint.USEast2);
var config = await appConfigService.GetConfigurationAsync<ApiPlemsiDto>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliedroEDS", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
              
    });
});
var app = builder.Build();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.UseCors("PoliedroEDS");
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
