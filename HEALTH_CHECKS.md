# Health Check Implementation

## Overview

This implementation adds health checks for external services, following the existing Tolgee pattern. The health checks are accessible via REST endpoints and provide monitoring capabilities for external dependencies.

## Implemented Health Checks

### Keycloak Health Check
- **Service**: `KeycloakHealthCheckService`
- **Endpoint Tested**: `{KeycloakUri}/realms/{Realm}/.well-known/openid_configuration`
- **Method**: Uses OpenID Connect discovery endpoint to verify Keycloak availability
- **Configuration Required**:
  - `Keycloak:KeycloakUri` - Base URL of Keycloak server
  - `Keycloak:Realm` - Realm name (defaults to "AppEDS")

### Existing Health Checks
- **MySQL Database**: Connection test to configured database
- **Redis**: Connection test to Redis cache
- **Tolgee**: Translation service availability check

## Accessing Health Checks

### Health Check Endpoints

1. **Simple Health Check**: `GET /health`
   - Returns overall system health status
   - Includes all configured health checks

2. **Detailed Health UI**: `GET /health-ui`
   - Web interface for monitoring health status
   - Real-time updates and historical data

### Response Format

```json
{
  "status": "Healthy|Degraded|Unhealthy",
  "totalDuration": "00:00:00.1234567",
  "entries": {
    "sql": {
      "status": "Healthy",
      "duration": "00:00:00.0123456"
    },
    "redis": {
      "status": "Healthy", 
      "duration": "00:00:00.0234567"
    },
    "Service Health Check Tolgee": {
      "status": "Healthy",
      "duration": "00:00:00.0345678"
    },
    "Service Health Check Keycloak": {
      "status": "Healthy",
      "duration": "00:00:00.0456789"
    }
  }
}
```

## Configuration

Add the following to your `appsettings.json`:

```json
{
  "Keycloak": {
    "KeycloakUri": "https://your-keycloak-instance.com",
    "Realm": "your-realm-name"
  }
}
```

## Testing

The Keycloak health check includes comprehensive unit tests covering:
- Successful health check scenarios
- Configuration validation
- Error handling for service unavailability
- Exception handling for network issues

Run tests with:
```bash
dotnet test
```

## Implementation Details

- Follows the same pattern as the existing Tolgee health check
- Uses `IHttpClientFactory` for HTTP client management
- Implements `IHealthCheck` interface from ASP.NET Core
- Configured with 30-second timeout for external calls
- Proper error handling and status reporting

## Extending with Additional Services

To add health checks for other external services:

1. Create a new service class implementing `IHealthCheck`
2. Add the service to health checks in `Program.cs`:
   ```csharp
   .AddCheck<YourHealthCheckService>("Service Health Check YourService")
   ```
3. Configure any required HTTP clients or dependencies
4. Add comprehensive unit tests