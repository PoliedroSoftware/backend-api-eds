using System.Net;

namespace Poliedro.Eds.Api.Tests.IntegrationTests;

public class ApiIntegrationTests
{
    [Fact]
    public void ApiProject_CanBeReferenced()
    {
        // This test verifies that the API project can be referenced from test project
        // and ensures the test infrastructure is properly configured
        var programType = typeof(Program);
        
        Assert.NotNull(programType);
        Assert.Equal("Program", programType.Name);
    }
}
