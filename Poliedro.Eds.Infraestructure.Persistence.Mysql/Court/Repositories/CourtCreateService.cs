using Poliedro.Eds.Application.Court.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Infraestructure.Persistence.Mysql.Context;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Court.Repositories;

public class CourtCreateService(DataBaseContext context) : ICourtDomainService
{
    public async Task<Result<VoidResult, Error>> CreateAsync(CourtEntity courtEntity)
    {
        await context.Court.AddAsync(courtEntity);
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return CourtErrorBuilder.CourtCreationException();

        return VoidResult.Instance;
    }
}
