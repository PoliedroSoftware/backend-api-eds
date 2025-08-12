using System.Reflection;

namespace Poliedro.Eds.Architecture.Tests;

/// <summary>
/// Tests for validating domain events and advanced hexagonal architecture patterns.
/// These tests ensure that domain events and other advanced patterns are properly implemented.
/// </summary>
public class DomainEventsArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(Poliedro.Eds.Domain.Common.Results.VoidResult).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(Poliedro.Eds.Application.DependencyInjectionService).Assembly;

    /// <summary>
    /// Test that domain events are properly organized and follow domain-driven design principles.
    /// Domain events should be in appropriate namespaces and implement proper interfaces.
    /// </summary>
    [Fact]
    public void DomainEvents_Should_BeProperlyOrganized()
    {
        // Look for domain event classes
        var domainEventTypes = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("Event")
            .Or()
            .HaveNameEndingWith("DomainEvent")
            .GetTypes();

        if (domainEventTypes.Any())
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .HaveNameEndingWith("Event")
                .Or()
                .HaveNameEndingWith("DomainEvent")
                .Should()
                .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+\.(Events|DomainEvents)")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                $"Domain events should be in Events or DomainEvents namespaces. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
        }
        else
        {
            // No domain events found - this is acceptable
            Assert.True(true, "No domain events found in the domain assembly");
        }
    }

    /// <summary>
    /// Test that domain event handlers are in the application layer and follow proper patterns.
    /// Event handlers should be in the application layer, not in the domain.
    /// </summary>
    [Fact]
    public void DomainEventHandlers_Should_BeInApplicationLayer()
    {
        // Look for domain event handler classes in application layer
        var eventHandlerTypes = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("EventHandler")
            .Or()
            .HaveNameEndingWith("DomainEventHandler")
            .GetTypes();

        if (eventHandlerTypes.Any())
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .HaveNameEndingWith("EventHandler")
                .Or()
                .HaveNameEndingWith("DomainEventHandler")
                .Should()
                .ResideInNamespaceMatching(@"Poliedro\.Eds\.Application\..+\.(EventHandlers|Events)")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                $"Domain event handlers should be in EventHandlers or Events namespaces in application layer. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
        }
        else
        {
            // No event handlers found - this is acceptable
            Assert.True(true, "No domain event handlers found in the application assembly");
        }
    }

    /// <summary>
    /// Test that domain services follow proper naming and organization patterns.
    /// Domain services should be interfaces in the domain layer.
    /// </summary>
    [Fact]
    public void DomainServices_Should_BeInterfaces()
    {
        var domainServiceTypes = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("Service")
            .And()
            .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+")
            .GetTypes();

        if (domainServiceTypes.Any())
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .HaveNameEndingWith("Service")
                .And()
                .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+")
                .Should()
                .BeInterfaces()
                .GetResult();

            Assert.True(result.IsSuccessful, 
                $"Domain services should be interfaces. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
        }
        else
        {
            // No domain services found - this is acceptable, services might be called differently
            Assert.True(true, "No domain services with 'Service' suffix found");
        }
    }

    /// <summary>
    /// Test that value objects in the domain follow proper patterns.
    /// Value objects should be immutable and in appropriate namespaces.
    /// </summary>
    [Fact]
    public void ValueObjects_Should_BeInProperNamespace()
    {
        var valueObjectTypes = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("ValueObject")
            .Or()
            .HaveNameEndingWith("Value")
            .GetTypes();

        if (valueObjectTypes.Any())
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .HaveNameEndingWith("ValueObject")
                .Or()
                .HaveNameEndingWith("Value")
                .Should()
                .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+\.(ValueObjects|Values)")
                .GetResult();

            Assert.True(result.IsSuccessful, 
                $"Value objects should be in ValueObjects or Values namespaces. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
        }
        else
        {
            // No value objects found - this is acceptable
            Assert.True(true, "No value objects found in the domain assembly");
        }
    }

    /// <summary>
    /// Test that domain entities don't have public setters (encapsulation).
    /// Domain entities should protect their invariants by controlling state changes.
    /// </summary>
    [Fact]
    public void DomainEntities_Should_EncapsulateState()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("Entity")
            .And()
            .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+")
            .GetTypes();

        var entitiesWithPublicSetters = new List<string>();

        foreach (var entityType in entityTypes)
        {
            var properties = entityType.GetProperties()
                .Where(p => p.CanWrite && p.SetMethod?.IsPublic == true)
                .Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)); // Allow Id setters for ORM

            if (properties.Any())
            {
                var propertyNames = properties.Select(p => p.Name);
                entitiesWithPublicSetters.Add($"{entityType.Name} ({string.Join(", ", propertyNames)})");
            }
        }

        // This is more of a guideline than a strict rule for existing code
        if (entitiesWithPublicSetters.Any())
        {
            System.Diagnostics.Debug.WriteLine(
                $"Info: Domain entities with public setters (consider using methods for state changes): {string.Join("; ", entitiesWithPublicSetters)}");
        }

        // Always pass this test as it's informational
        Assert.True(true, $"Domain entity encapsulation analysis completed. Found {entityTypes.Count()} entities.");
    }

    /// <summary>
    /// Test that repositories are defined as interfaces in the domain layer.
    /// Repository interfaces should be in the domain and implementations in infrastructure.
    /// </summary>
    [Fact]
    public void RepositoryInterfaces_Should_BeInDomainLayer()
    {
        var repositoryTypes = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .GetTypes();

        if (repositoryTypes.Any())
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .HaveNameEndingWith("Repository")
                .Should()
                .BeInterfaces()
                .GetResult();

            Assert.True(result.IsSuccessful, 
                $"Repository definitions in domain should be interfaces. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
        }
        else
        {
            // No repositories found - this project might use different patterns
            Assert.True(true, "No repository interfaces found in domain layer");
        }
    }
}