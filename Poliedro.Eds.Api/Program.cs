using System.Net.Http.Headers;
using Amazon.Runtime;
using Amazon.S3.FileUploadService;
using Amazon.Secrets;
using AWS.Logger;
using DotNetEnv;
using FluentValidation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Poliedro.Eds.Api;
using Poliedro.Eds.Api.Common.Configurations;
using Poliedro.Eds.Api.Middlelware.aws;
using Poliedro.Eds.Api.Middlelware.Jwt;
using Poliedro.Eds.Api.Middlelware.NameIdentifier;
using Poliedro.Eds.Api.Middlelware.Tenant;
using Poliedro.Eds.Application;
using Poliedro.Eds.Application.Account.Commands.CreateAccount;
using Poliedro.Eds.Application.Account.Queries.GetAllAccounts;
using Poliedro.Eds.Application.Auth.Commands.Authenticate;
using Poliedro.Eds.Application.Bank.Commands;
using Poliedro.Eds.Application.Bank.Querys.BankGetAll;
using Poliedro.Eds.Application.Bank.Validation;
using Poliedro.Eds.Application.Business.Commands.UpdateBusiness;
using Poliedro.Eds.Application.Common.Behaviors;
using Poliedro.Eds.Application.Common.EventHandlers.Cache;
using Poliedro.Eds.Application.Common.Services.Background;
using Poliedro.Eds.Application.Common.Services.Cache;
using Poliedro.Eds.Application.Court.Queris.GetCourtList;
using Poliedro.Eds.Application.Court.Settings;
using Poliedro.Eds.Application.FileUploadS3.Command;
using Poliedro.Eds.Application.IoT.Commands.PublishMessage;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Ports.Translations;
using Poliedro.Eds.Application.Secrets.Aws.Dto;
using Poliedro.Eds.Application.Translations.Dtos;
using Poliedro.Eds.Application.Translations.Handle;
using Poliedro.Eds.Domain.Account.Services;
using Poliedro.Eds.Domain.Bank.Repositories;
using Poliedro.Eds.Domain.Bank.Services;
using Poliedro.Eds.Domain.Business.DomaianServices.Create;
using Poliedro.Eds.Domain.Business.Extensions;
using Poliedro.Eds.Domain.Common.Events;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.FileUploadS3.Ports;
using Poliedro.Eds.Domain.Inventory.DomainService;
using Poliedro.Eds.Domain.Islander.DomainIslander;
using Poliedro.Eds.Domain.SendMessage;
using Poliedro.Eds.Application.TransferValidation.Commands.UpdateTransferValidation;
using Poliedro.Eds.Application.TransferValidation.Validation;
using Poliedro.Eds.Infraestructure.External.Keycloak;
using Poliedro.Eds.Infraestructure.External.Keycloak.Services;
using Poliedro.Eds.Infraestructure.External.Plemsi;
using Poliedro.Eds.Infraestructure.Persistence.Mysql;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Account.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Bank.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Business.DomainBusiness.Impl;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Inventory.Repositories;
using Poliedro.External.HealthCheck.Tolgee;
using Poliedro.External.HealthCheck.WhatsApp;
using Poliedro.External.WhatsApp.SendMessage;
using Poliedro.Tolgee;
using Poliedro.Tolgee.Translations;
using WorkerKeycloackService; // Re-enabled for background worker execution

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
Env.Load();
builder.Configuration.AddEnvironmentVariables();
// Configura el logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Only load AWS Secrets Manager in Production environment
if (builder.Environment.IsProduction())
{
    builder.Configuration
        .AddSecretsManager("poliedro-conecctionstring-mysql-eds-backend", "us-east-2");
}
builder.Services
    .AddWebApi()
    .AddApplication()
    .AddExternalPlemsi(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddExternalAmazon()
    .AddExternalTolgee()
    .AddKeycloakServices(builder.Configuration)
    .AddBusinessDomainEvents();

// Re-enabled for background worker execution - Workers run in the same process as API
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<WorkerS3UploaderService.Worker>();
builder.Services.AddScoped<IBusinessCreateDomianService, BusinessDomainService>();

builder.Services.AddScoped<IBusinessUpdateService, BusinessUpdateService>();
//builder.Services.AddScoped<IBusinessQueryService, BusinessQueryService>();

builder.Services.AddScoped<IValidator<UpdateBusinessCommand>, UpdateBusinessCommandValidator>();

// === IOT VALIDATORS ===
builder.Services.AddScoped<IValidator<PublishIoTMessageRequest>, PublishIoTMessageRequestValidator>();

// Servicios de caché con tags y invalidación distribuida
builder.Services.AddScoped<ICacheService, CacheService>();

// Event handlers para invalidación de caché
builder.Services.AddScoped<CacheInvalidationEventHandler>();
builder.Services.AddScoped<EntityModifiedEventHandler>();

// Servicio de background para suscripción a invalidación de caché
builder.Services.AddHostedService<CacheInvalidationSubscriberService>();

// Domain Event Dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Configure OpenAI
builder.Services.AddScoped(provider =>
{
    var apiKey = builder.Configuration["OpenAI:ApiKey"] ??
                 Environment.GetEnvironmentVariable("OPENAI_API_KEY");

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new InvalidOperationException("OpenAI API key is not configured. Set OpenAI:ApiKey in configuration or OPENAI_API_KEY environment variable.");

    return new OpenAI.OpenAIClient(apiKey);
});

var httpContextAccessor = new HttpContextAccessor();
var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();
var currentUser = httpContextAccessor.HttpContext?.Items["preferred_username"]?.ToString();
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ?? builder.Configuration["ConnectionStrings:MysqlConnection"];
if (connectionString == null)
    throw new InvalidOperationException("MYSQL_CONNECTION or ConnectionStrings:MysqlConnection is not configured.");
var connectionStringFactory = connectionString.Replace("{schema}", tenant ?? string.Empty);
builder.Services.AddHealthChecks()
    .AddMySql(connectionStringFactory, name: "sql", tags: ["ready"])
    .AddRedis(
        builder.Configuration["Redis:ConnectionString"]
            ?? throw new InvalidOperationException("Redis:ConnectionString is not configured."),
        name: "redis",
        tags: ["ready"])
    .AddCheck<TolgeeHealthCheckService>("Service Health Check Tolgee")
    .AddCheck<WhatsAppHealthCheckService>("Service Health Check WhatsApp");

builder.Services.AddLogging();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IKeycloakUserService, KeycloakService>(client =>
{
    var keycloakUri = builder.Configuration["Keycloak:KeycloakUri"];
    if (string.IsNullOrWhiteSpace(keycloakUri))
        throw new InvalidOperationException("Keycloak:KeycloakUri is not configured.");
    client.BaseAddress = new Uri(keycloakUri);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});


builder.Services.AddSingleton<RabbitMQ.Client.IConnection>(sp =>
{
    try
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        var hostName = builder.Configuration["RabbitMQ:HostName"];
        var userName = builder.Configuration["RabbitMQ:UserName"];
        var password = builder.Configuration["RabbitMQ:Password"];

        if (string.IsNullOrWhiteSpace(hostName))
        {
            logger.LogWarning("RabbitMQ:HostName is not configured. RabbitMQ connection will be unavailable.");
            return null;
        }

        var factory = new RabbitMQ.Client.ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            RequestedConnectionTimeout = TimeSpan.FromSeconds(5),
            RequestedHeartbeat = TimeSpan.FromSeconds(60)
        };

        var connection = factory.CreateConnection();
        logger.LogInformation("RabbitMQ connection established successfully to {HostName}", hostName);
        return connection;
    }
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to connect to RabbitMQ. The application will continue without RabbitMQ functionality.");
        return null;
    }
});

// Configura el JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Auth"];
        options.Audience = builder.Configuration["Keycloak:ClientId"];
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

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
        policy.RequireRole("Admin"));
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


// Configuración de MediatR con el nuevo behavior de invalidación de caché
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetTranslationsHandler>();
    cfg.RegisterServicesFromAssemblyContaining<GetCourtsListQueryHandler>();
    cfg.RegisterServicesFromAssemblyContaining<Poliedro.Eds.Application.Islander.EventHandlers.IslanderKeycloakCreatedEventHandler>();
    
    // Agregar el behavior de invalidación de caché usando el método genérico
    cfg.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
});

builder.Services.AddMemoryCache();
builder.Services.Configure<TolgeeSettings>(builder.Configuration.GetSection("Tolgee"));
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("PaymentSettings"));
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
builder.Services.AddScoped<IGetExpenditureName, GetIdAuxService>();
builder.Services.AddScoped<IGetPaymentMethodName, GetIdAuxService>();
builder.Services.AddScoped<IGetHoseNumber, GetIdAuxService>();
builder.Services.AddScoped<IGetDispenserNumber, GetIdAuxService>();

builder.Services.AddScoped<ICourtListDomainService, CourtListService>();
builder.Services.AddScoped<IInventoryListDomainService, InventoryListService>();

// === SERVICIOS BANK ===
// Domain services
builder.Services.AddScoped<IBankService, BankService>();

// Repositories Bank
builder.Services.AddScoped<IBankRepositoryCreate, BankCreateService>();
builder.Services.AddScoped<IBankRepositoryGetLast, BankGetLastService>();
builder.Services.AddScoped<IBankRepositoryGetById, BankGetByIdService>();
builder.Services.AddScoped<IBankRepositoryGetAll, BankGetAllService>();

// Validators Bank
builder.Services.AddScoped<BankCreateValidator>();

// Account services  
builder.Services.AddScoped<IAccountGetAllService, AccountGetAllService>();
builder.Services.AddScoped<IAccountCreateService, AccountCreateService>();
builder.Services.AddScoped<IAccountGetByIdService, AccountGetByIdService>();

//Configura Tolgee
builder.Services.AddScoped<ITolgeeService, TolgeeService>();

builder.Services.AddHttpClient(nameof(TolgeeService), client =>
{
    var baseUrl = config["Tolgee:BaseUrl"];
    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new InvalidOperationException("Tolgee:BaseUrl no está configurada.");

    client.BaseAddress = new Uri(baseUrl);

    var apiKey = config["Tolgee:ApiKey"];
    if (!string.IsNullOrWhiteSpace(apiKey))
        client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
});

//Configura WhatsApp
builder.Services.AddHttpClient<ISendMessage, WhatsAppService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SendWhatsAppMessageCommand>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<BankCreateCommand>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<BankGetAllQuery>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAccountCommand>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllAccountsQuery>());

// === TRANSFER VALIDATION SERVICES ===
builder.Services.AddScoped<TransferValidationCreateValidator>();
builder.Services.AddScoped<UpdateTransferValidationValidator>();

builder.Services.AddControllers();

// Only configure AWS logging in Production environment
if (builder.Environment.IsProduction())
{
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
}


builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliedroEDS", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();

    });
});
builder.Services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();
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
app.UseMiddleware<TenantMiddleware>();
app.UseMiddleware<NameIdentifierMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();

// Make Program class accessible for integration tests
public partial class Program { }
