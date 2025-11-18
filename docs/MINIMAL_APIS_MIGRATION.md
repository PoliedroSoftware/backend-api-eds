# Minimal APIs Migration Documentation

## Overview

This document describes the migration from traditional ASP.NET Core controllers to minimal APIs in the backend-api-eds project.

## Migration Summary

**Status**: ✅ Complete - Controllers Disabled, Minimal APIs Active

**Migration Date**: 2025-11-16  
**Controllers Disabled**: 2025-01-18

### What Changed

- **Before**: Traditional MVC-style controllers in `/Controllers/v1/` folder
- **After**: Minimal API endpoints in `/Endpoints/v1/` folder
- **Current State**: Minimal APIs only - controllers commented out to resolve route conflicts

## Current Configuration

### Active Routes
```csharp
// In Program.cs
app.MapApiEndpoints();  // ✅ ACTIVE - New minimal APIs
// app.MapControllers();  // ❌ COMMENTED OUT - To prevent duplicate routes
```

### Why Controllers Were Disabled

During testing, we discovered that having both controllers and minimal APIs active caused `AmbiguousMatchException` errors because both were registering the same routes. This resulted in 30 failing integration tests.

**Fix Applied**: Commented out `app.MapControllers()` in `Program.cs` to use only minimal APIs.

**Result**: 
- Before: 30/158 tests failing due to route conflicts
- After: 158/158 tests passing (100% success rate)

### Additional Fixes

To achieve 100% test success rate, we also:

1. **Mocked ITolgeeService** in integration tests
   - Previously attempted real HTTP calls to external Tolgee service
   - Now returns mock data in test environment
   - File: `Poliedro.Eds.Api.Tests/Infrastructure/CustomWebApplicationFactory.cs`

## Architecture

### Folder Structure

```
Poliedro.Eds.Api/
├── Controllers/v1/          # Original controllers (INACTIVE - preserved for reference)
├── Endpoints/               # New minimal APIs (ACTIVE)
│   ├── EndpointExtensions.cs   # Central registration
│   └── v1/                      # V1 API endpoints
│       ├── AuthEndpoints.cs
│       ├── AccountEndpoints.cs
│       ├── BankEndpoints.cs
│       └── ... (32 more endpoint files)
└── Program.cs              # Updated to use minimal APIs only
```

### Endpoint Organization

Each endpoint file follows this pattern:

```csharp
public static class {Entity}Endpoints
{
    public static IEndpointRouteBuilder Map{Entity}Endpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/{route}")
            .WithTags("{Entity}")
            .RequireAuthorization("Policy");

        group.MapGet("", GetAll).WithName("...").WithSummary("...");
        group.MapPost("", Create).WithName("...").WithSummary("...");
        // ... more endpoints

        return app;
    }

    private static async Task<IResult> GetAll(...)  { ... }
    private static async Task<IResult> Create(...)  { ... }
    // ... handler methods
}
```

## Migrated Endpoints

### Complete List (35 Controllers)

| Controller | Endpoints | Route | Status |
|------------|-----------|-------|--------|
| Auth | 1 | `/api/v1/auth` | ✅ |
| Account | 3 | `/api/v1/account` | ✅ |
| Bank | 4 | `/api/v1/bank` | ✅ |
| Business | 5 | `/api/v1/business` | ✅ |
| Capacity | 4 | `/api/v1/capacity` | ✅ |
| CompartimentCapacity | 4 | `/api/v1/compartiment-capacity` | ✅ |
| Court | 4 | `/api/v1/court` | ✅ |
| CourtDispensersInventory | 4 | `/api/v1/court-dispensers-inventory` | ✅ |
| Dispensers | 4 | `/api/v1/dispensers` | ✅ |
| Eds | 4 | `/api/v1/eds` | ✅ |
| EdsTank | 4 | `/api/v1/eds-tank` | ✅ |
| Expenditures | 4 | `/api/v1/expenditures` | ✅ |
| FileUploadS3 | 1 | `/api/v1/files` | ✅ |
| Hose | 5 | `/api/v1/hose` | ✅ |
| HoseHistory | 4 | `/api/v1/hose-history` | ✅ |
| Inventory | 1 | `/api/v1/inventory` | ✅ |
| IoT | 1 | `/api/v1/iot` | ✅ |
| Island | 4 | `/api/v1/island` | ✅ |
| Islander | 4 | `/api/v1/islander` | ✅ |
| OpenAI | 2 | `/api/v1/openai` | ✅ |
| Phone | 3 | `/api/v1/phone` | ✅ |
| Product | 4 | `/api/v1/product` | ✅ |
| ProductType | 4 | `/api/v1/product-type` | ✅ |
| Provider | 4 | `/api/v1/provider` | ✅ |
| RegisterShift | 1 | `/api/v1/register-shift` | ✅ |
| WhatsApp | 1 | `/api/v1/send-message` | ✅ |
| Shopping | 4 | `/api/v1/shopping` | ✅ |
| ShoppingProduct | 4 | `/api/v1/shopping-product` | ✅ |
| ShoppingProductInventory | 3 | `/api/v1/shopping-product-inventory` | ✅ |
| StrongBox | 5 | `/api/v1/strong-box` | ✅ |
| Tank | 4 | `/api/v1/tank` | ✅ |
| TransferValidation | 2 | `/api/v1/transfer-validation` | ✅ |
| Translations | 1 | `/api/v1/translations` | ✅ |
| TypeOfCollection | 4 | `/api/v1/type-of-collection` | ✅ |
| SetupWizard | 1 | `/api/v1/setup-wizard` | ✅ |

**Total**: 100+ endpoints across 35 controllers

## Key Features Preserved

### 1. Authorization
- All authorization policies maintained (`AdminOnly`, `AdminOrIslander`)
- JWT authentication continues to work identically

### 2. MediatR Integration
- All commands and queries use MediatR
- Same handler classes, no changes to application layer

### 3. Swagger Documentation
- Endpoint summaries and descriptions preserved via `WithSummary()`/`WithDescription()`
- Response type annotations via `.Produces<T>()`

### 4. Validation
- FluentValidation continues to work
- Validation errors handled in endpoint methods

### 5. Dependency Injection
- All services injected via method parameters
- Same service registrations in `Program.cs`

## Benefits of Minimal APIs

1. **Performance**: Reduced overhead compared to controllers
2. **Simplicity**: Less boilerplate code
3. **Readability**: Clearer request handling flow
4. **Modern**: Aligns with ASP.NET Core best practices
5. **Testability**: Easier to test individual endpoints

## Backward Compatibility

### Hybrid Mode (No Longer Active)
Previously both controllers and minimal APIs coexisted. This has been disabled to prevent route conflicts.

### Route Compatibility
- All routes remain identical (`/api/v1/...`)
- Request/response formats unchanged
- Client applications require no changes

## Testing

### Build Status
✅ Project builds successfully with all minimal API endpoints

### Latest Test Results (2025-01-18)

```
Test summary: total: 158
- Passed: 158 (100%)
- Failed: 0 (0%)
- Skipped: 0

All route conflicts resolved ✅
All external dependencies mocked ✅
```

### Test Fixes Applied

1. **Route Conflicts**: Disabled MapControllers() to prevent AmbiguousMatchException
2. **External Dependencies**: Mocked ITolgeeService in test environment
3. **Result**: 100% test success rate

### Known Test Considerations

#### Redis Connection Warnings
- Tests show Redis connection timeout warnings in logs
- These are non-blocking and expected in test environment
- Configured with `abortConnect=false` to continue on connection failure
- Does not affect test success

## Rollback Plan

If issues arise, rollback is simple:

1. Uncomment `app.MapControllers()` in `Program.cs`
2. Comment out `app.MapApiEndpoints()`
3. The original controllers remain untouched in `/Controllers/v1/` folder
4. Revert mock changes in `CustomWebApplicationFactory.cs` if needed

**Note**: You cannot run both at the same time - choose one or the other to avoid route conflicts.

## Migration Challenges & Solutions

### Challenge 1: Route Conflicts (Resolved)
**Problem**: Both controllers and minimal APIs registered same routes

**Solution**: Disabled controllers by commenting `app.MapControllers()`

**Result**: 30 tests fixed immediately

### Challenge 2: External Service Dependencies (Resolved)
**Problem**: Tests failed when calling external Tolgee service

**Solution**: Mocked `ITolgeeService` in `CustomWebApplicationFactory`

**Result**: All 158 tests now pass

### Challenge 3: Return Type Mismatches
**Problem**: Some commands return `VoidResult` instead of DTOs

**Solution**: Changed from `.Created($"/url/{id}", result.Value)` to `.Created()`

### Challenge 4: Namespace Variations
**Problem**: Query namespaces differ (e.g., `Queries` vs `Querys`)

**Solution**: Checked actual controller implementations for correct namespaces

### Challenge 5: HttpContext Access
**Problem**: Some endpoints need `HttpContext` (e.g., user claims)

**Solution**: Added `HttpContext` parameter to endpoint methods

## Next Steps

### Phase 1: Monitoring (Current)
- ✅ All tests passing
- ✅ Documentation updated
- Monitor production performance
- Gather feedback from development team

### Phase 2: Cleanup (Future - Optional)
- [ ] Remove `/Controllers/` folder (after production validation period)
- [ ] Update any remaining controller-specific documentation
- [ ] Consider mocking Redis to eliminate test warnings

### Phase 3: Optimization (Optional)
- [ ] Add endpoint filters for cross-cutting concerns
- [ ] Implement rate limiting per endpoint
- [ ] Add endpoint-specific caching strategies

## Contact & Support

For questions about this migration:
- Review this documentation
- Check endpoint implementation in `/Endpoints/v1/` folder
- Refer to test fixes in `docs/TEST_FIXES_SUMMARY.md`
- Refer to original controller logic in `/Controllers/v1/` folder (for reference)

## Related Documentation

- [Test Fixes Summary](TEST_FIXES_SUMMARY.md) - Detailed information about test corrections
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis) - Microsoft documentation

---

**Last Updated**: 2025-01-18  
**Migration Status**: Complete - Minimal APIs Active, Controllers Disabled  
**Test Status**: 100% passing (158/158)
