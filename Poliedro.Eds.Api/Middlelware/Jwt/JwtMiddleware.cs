using System.Security.Claims;

namespace Poliedro.Eds.Api.Middlelware.Jwt
{
    public class JwtMiddleware(
        
        RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var identity = user.Identity as ClaimsIdentity;

                var resourceAccessClaim = user.FindFirst("resource_access");
                if (resourceAccessClaim != null)
                {
                    var resourceAccess = System.Text.Json.JsonDocument.Parse(resourceAccessClaim.Value);

                    if (resourceAccess.RootElement.TryGetProperty("application-eds", out var appAccess) &&
                        appAccess.TryGetProperty("roles", out var roles))
                    {
                        foreach (var role in roles.EnumerateArray())
                        {
                            if (!identity.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == role.GetString()))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()));
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
