namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Context
{
    public interface ITenantDbContextFactory
    {
        DataBaseContext CreateDbContext();
        DataBaseContext CreateDbContext(string tenant);
    }
}
