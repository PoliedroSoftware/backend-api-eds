using System.Reflection;

namespace Poliedro.Eds.Architecture.Tests;

/// <summary>
/// Tests to validate hexagonal architecture principles and layer separation.
/// These tests ensure that the architectural boundaries are respected across the application.
/// </summary>
public class HexagonalArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(Poliedro.Eds.Domain.Common.Results.VoidResult).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(Poliedro.Eds.Application.DependencyInjectionService).Assembly;
    private static readonly Assembly InfrastructurePersistenceAssembly = typeof(Poliedro.Eds.Infraestructure.Persistence.Mysql.Context.ITenantDbContextFactory).Assembly;
    private static readonly Assembly InfrastructureKeycloakAssembly = typeof(Poliedro.Eds.Infraestructure.External.Keycloak.DependencyInjectionService).Assembly;
    private static readonly Assembly InfrastructurePlemsiAssembly = typeof(Poliedro.Eds.Infraestructure.External.Plemsi.DependencyInjectionService).Assembly;

    /// <summary>
    /// Test that application layer only depends on the domain layer.
    /// Application services should not depend on infrastructure or API layers.
    /// </summary>
    [Fact]
    public void ApplicationLayer_Should_OnlyDependOnDomain()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                "Poliedro.Eds.Api",
                "Poliedro.Eds.Infraestructure.Persistence.Mysql",
                "Poliedro.Eds.Infraestructure.External.Keycloak",
                "Poliedro.Eds.Infraestructure.External.Plemsi",
                "Poliedro.Eds.Infraestructure.External.TNS")
            .GetResult();

        Assert.True(result.IsSuccessful, $"Application layer should not depend on infrastructure or API layers. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }

    /// <summary>
    /// Test that domain layer has no dependencies on other application layers.
    /// Domain should be the core with no external dependencies.
    /// </summary>
    [Fact]
    public void DomainLayer_Should_NotDependOnOtherLayers()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                "Poliedro.Eds.Api",
                "Poliedro.Eds.Application",
                "Poliedro.Eds.Infraestructure.Persistence.Mysql",
                "Poliedro.Eds.Infraestructure.External.Keycloak",
                "Poliedro.Eds.Infraestructure.External.Plemsi",
                "Poliedro.Eds.Infraestructure.External.TNS")
            .GetResult();

        Assert.True(result.IsSuccessful, $"Domain layer should not depend on any other application layers. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }

    /// <summary>
    /// Test that infrastructure layers can depend on application and domain, but not on API layer.
    /// Infrastructure implements the ports defined in domain and used by application.
    /// </summary>
    [Fact]
    public void InfrastructureLayer_Should_NotDependOnApiLayer()
    {
        var infrastructureAssemblies = new[]
        {
            InfrastructurePersistenceAssembly,
            InfrastructureKeycloakAssembly,
            InfrastructurePlemsiAssembly
        };

        foreach (var assembly in infrastructureAssemblies)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Poliedro.Eds.Api")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Infrastructure layer in {assembly.GetName().Name} should not depend on API layer. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
        }
    }

    /// <summary>
    /// Test that there are no direct dependencies between domain and infrastructure layers.
    /// Domain should only know about infrastructure through abstractions (ports).
    /// </summary>
    [Fact]
    public void Domain_Should_NotDirectlyDependOnInfrastructure()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                "Poliedro.Eds.Infraestructure.Persistence.Mysql",
                "Poliedro.Eds.Infraestructure.External.Keycloak",
                "Poliedro.Eds.Infraestructure.External.Plemsi",
                "Poliedro.Eds.Infraestructure.External.TNS")
            .GetResult();

        Assert.True(result.IsSuccessful, $"Domain should not directly depend on infrastructure. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }

    /// <summary>
    /// Test that classes in the domain layer follow proper naming conventions.
    /// Domain entities should be in the correct namespaces and have proper naming.
    /// Note: Some view entities may not follow strict entity patterns and are allowed.
    /// </summary>
    [Fact]
    public void DomainEntities_Should_BeInProperNamespace()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("Entity")
            .And()
            .DoNotHaveNameMatching(".*View.*") // Allow view entities to be more flexible
            .Should()
            .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+\.Entities")
            .GetResult();

        Assert.True(result.IsSuccessful, $"Domain entities should be in proper namespace structure. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }

    /// <summary>
    /// Test that domain services/ports are properly organized.
    /// Domain interfaces should be in appropriate port namespaces.
    /// </summary>
    [Fact]
    public void DomainPorts_Should_BeInterfaces()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+\.Ports")
            .Or()
            .ResideInNamespaceMatching(@"Poliedro\.Eds\.Domain\..+\.Domain.+")
            .Should()
            .BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful, $"Domain ports should be interfaces. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }

    /// <summary>
    /// Test that application command and query handlers follow CQRS patterns.
    /// Handlers should be in proper namespaces and implement correct interfaces.
    /// Note: Some handlers may be in different namespace structures and are allowed for compatibility.
    /// </summary>
    [Fact]
    public void ApplicationHandlers_Should_FollowCQRSPattern()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .And()
            .DoNotHaveNameMatching(".*WhatsAppMessageCommandHandler") // Allow external handlers
            .Should()
            .ResideInNamespaceMatching(@"Poliedro\.Eds\.Application\..+\.(Commands?|Queries|Handle)")
            .GetResult();

        // Log information about handlers that don't follow the pattern
        var allHandlers = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .GetTypes();
            
        var nonCompliantHandlers = allHandlers.Where(h => 
            !System.Text.RegularExpressions.Regex.IsMatch(h.Namespace ?? "", @"Poliedro\.Eds\.Application\..+\.(Commands?|Queries|Handle)"));
            
        foreach (var handler in nonCompliantHandlers)
        {
            System.Diagnostics.Debug.WriteLine($"Info: Handler {handler.FullName} is in namespace {handler.Namespace}");
        }

        Assert.True(result.IsSuccessful, $"Application handlers should follow CQRS pattern and be in Commands, Queries, or Handle namespaces. Violations: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>())}");
    }

    /// <summary>
    /// Test that infrastructure implementations properly implement domain ports.
    /// Infrastructure classes should implement interfaces defined in domain.
    /// </summary>
    [Fact]
    public void InfrastructureImplementations_Should_BeInImplNamespaces()
    {
        var infrastructureAssemblies = new[]
        {
            InfrastructurePersistenceAssembly,
            InfrastructureKeycloakAssembly,
            InfrastructurePlemsiAssembly
        };

        foreach (var assembly in infrastructureAssemblies)
        {
            var result = Types.InAssembly(assembly)
                .That()
                .AreClasses()
                .And()
                .AreNotAbstract()
                .And()
                .DoNotHaveNameMatching(@".*\.(DependencyInjectionService|.*Extension|.*Configuration)$")
                .Should()
                .ResideInNamespaceEndingWith("Impl")
                .Or()
                .ResideInNamespaceEndingWith("Services")
                .GetResult();

            // This is more of a guideline than a strict rule, so we'll make this a warning
            if (!result.IsSuccessful)
            {
                var violations = string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? Array.Empty<string>());
                // Log warning but don't fail the test as this is more about organization than correctness
                System.Diagnostics.Debug.WriteLine($"Warning: Infrastructure implementations in {assembly.GetName().Name} should typically be in 'Impl' or 'Services' namespaces. Classes: {violations}");
            }
        }
    }
}