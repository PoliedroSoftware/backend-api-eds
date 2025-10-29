using WireMock.Server;

namespace Poliedro.Eds.Api.Tests.Infrastructure;

public class WireMockFixture : IDisposable
{
    public WireMockServer Server { get; }
    private bool _disposed = false;

    public WireMockFixture()
    {
        try
        {
            Server = WireMockServer.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start WireMock server: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            try
            {
                Server?.Stop();
                Server?.Dispose();
            }
            catch (Exception ex)
            {
                // Log but don't throw to avoid masking test failures
                Console.WriteLine($"Error disposing WireMock server: {ex.Message}");
            }
        }

        _disposed = true;
    }
}
