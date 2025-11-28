using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Commands.CreatePosOfSale;

public record CreatePosOfSaleDetailsCommand(CreatePosOfSaleDetailsRequestDto Request) : IRequest<Result<VoidResult, Error>>;

