using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poliedro.Eds.Domain.Ports;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Adapter;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.External.Plemsi
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddExternalPlemsi(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ?? configuration.GetConnectionString("MysqlConnection");
            services.AddDbContext<DataBaseContext>(
                options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
            ));

            services.AddTransient<IMessageProvider, MessageProvider>();

            return services;
        }
    }
}
