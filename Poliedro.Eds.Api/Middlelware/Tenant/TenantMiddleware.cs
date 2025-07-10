using System.Security.Claims;

namespace Poliedro.Eds.Api.Middlelware.Tenant;


public class TenantMiddleware(
    RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var identity = user.Identity as ClaimsIdentity;

            var tenantClaim = user.FindFirst("preferred_username")?.Value;

            if (!string.IsNullOrWhiteSpace(tenantClaim))
            {
                context.Items["tenant"] = tenantClaim;
              
            }
        }

        await _next(context);

    }
}
