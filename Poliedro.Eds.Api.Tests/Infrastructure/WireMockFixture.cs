using WireMock.Server;

namespace Poliedro.Eds.Api.Tests.Infrastructure;

public class WireMockFixture : IDisposable
{
    public WireMockServer Server { get; }

    public WireMockFixture()
    {
        Server = WireMockServer.Start();
    }

    public void Dispose()
    {
        Server?.Stop();
        Server?.Dispose();
    }
}
