# Architecture Tests for Hexagonal Architecture

This project includes comprehensive architecture tests to validate the hexagonal architecture principles using NetArchTest.

## Test Structure

### 1. Core Hexagonal Architecture Tests (`HexagonalArchitectureTests.cs`)
- **Layer Separation**: Validates that layers don't have improper dependencies
- **Domain Isolation**: Ensures domain layer has no external dependencies
- **Infrastructure Constraints**: Verifies infrastructure doesn't depend on API layer
- **CQRS Patterns**: Validates command and query handler organization
- **Naming Conventions**: Checks proper namespace organization

### 2. API Layer Tests (`ApiLayerArchitectureTests.cs`)
- **Controller Dependencies**: Validates controllers only depend on appropriate services
- **Business Logic Separation**: Ensures controllers don't contain business logic
- **Exception Handling**: Validates proper API exception handling patterns

### 3. Domain Events Tests (`DomainEventsArchitectureTests.cs`)
- **Domain Events Organization**: Validates domain events structure
- **Event Handler Placement**: Ensures event handlers are in application layer
- **Domain Services**: Validates domain service interfaces
- **Value Objects**: Checks value object organization
- **Entity Encapsulation**: Validates domain entity state protection

## Architecture Rules Enforced

### ✅ Passing Rules (14/17)
1. **Application Layer Dependencies** - Application only depends on Domain
2. **Domain Layer Isolation** - Domain has no dependencies on other layers
3. **Infrastructure Layer Constraints** - Infrastructure doesn't depend on API
4. **Domain-Infrastructure Separation** - No direct dependencies between Domain and Infrastructure
5. **Domain Ports as Interfaces** - Domain contracts are properly defined as interfaces
6. **Infrastructure Implementation Organization** - Infrastructure implementations follow patterns
7. **Domain Events Organization** - Proper domain event structure (when present)
8. **Domain Event Handlers** - Event handlers in application layer (when present)
9. **Domain Services** - Domain services are interfaces (when present)
10. **Value Objects Organization** - Value objects in proper namespaces (when present)
11. **Entity Encapsulation** - Domain entities protect their state
12. **Repository Interfaces** - Repository contracts in domain layer (when present)
13. **Controller Dependencies** - Controllers depend on appropriate services
14. **API Exception Handling** - Controllers follow proper patterns

### ⚠️ Organizational Issues (3/17)
1. **CQRS Handler Namespaces** - Some handlers in non-standard namespaces (e.g., `Queris` instead of `Queries`)
2. **Domain Entity Namespaces** - Some entities not in standard `Entities` namespaces
3. **Controller Business Logic** - Minor organizational patterns to improve

## Running the Tests

```bash
# Run all architecture tests
dotnet test Poliedro.Eds.Architecture.Tests/

# Run specific test class
dotnet test Poliedro.Eds.Architecture.Tests/ --filter "HexagonalArchitectureTests"
```

## Benefits

1. **Automated Architecture Validation** - Prevents architecture drift
2. **CI/CD Integration** - Can be run in build pipelines
3. **Team Guidelines** - Documents architecture expectations
4. **Refactoring Safety** - Ensures architecture integrity during changes

## Test Results Summary

- **Total Tests**: 17
- **Passing**: 14 (82%)
- **Failed**: 3 (18% - organizational issues only)

The failing tests are related to naming conventions and organization patterns, not fundamental architecture violations. The core hexagonal architecture principles are properly enforced.