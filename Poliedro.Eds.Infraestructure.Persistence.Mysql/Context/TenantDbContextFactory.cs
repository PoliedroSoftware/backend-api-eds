using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Audit;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

public class TenantDbContextFactory(
IHttpContextAccessor httpContextAccessor,
Microsoft.Extensions.Configuration.IConfiguration config) : ITenantDbContextFactory
{
    public DataBaseContext CreateDbContext()
    {

        var tenant = httpContextAccessor.HttpContext?.Items["tenant"]?.ToString();

        if (string.IsNullOrWhiteSpace(tenant))
            throw new InvalidOperationException("Tenant not found");

        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ?? config["ConnectionStrings:MysqlConnection"];
        var connectionStringFactory = connectionString.Replace("{schema}", tenant);

        var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>();
        optionsBuilder.UseMySql(connectionStringFactory, ServerVersion.AutoDetect(connectionStringFactory));

        return new AuditableDbContext(optionsBuilder.Options, httpContextAccessor);
    }
}
