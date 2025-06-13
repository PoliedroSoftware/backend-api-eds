using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Poliedro.Billing.Infraestructure.External.TNS
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddExternalTns(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
