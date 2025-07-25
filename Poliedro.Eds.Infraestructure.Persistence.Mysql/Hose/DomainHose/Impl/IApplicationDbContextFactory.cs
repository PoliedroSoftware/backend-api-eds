namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Hose.DomainHose.Impl
{
    internal interface IApplicationDbContextFactory
    {
        object CreateDbContext();
    }
}