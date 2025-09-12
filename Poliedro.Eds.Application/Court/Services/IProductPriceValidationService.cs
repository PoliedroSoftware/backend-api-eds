using Poliedro.Eds.Application.Court.Commands.CreateCourt;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Court.Services
{
    public interface IProductPriceValidationService
    {
        Task<Result<VoidResult, Error>> ValidateAndUpdateProductPricesAsync(
            IEnumerable<CourtDispenserCommand> courtDispensers,
            CancellationToken cancellationToken = default);
    }
}
