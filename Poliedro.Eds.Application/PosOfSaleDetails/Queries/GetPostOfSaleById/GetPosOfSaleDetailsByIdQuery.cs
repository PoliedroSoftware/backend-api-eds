using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.PosOfSaleDetails.Dtos;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Queries.GetPostOfSaleById;

public record GetPosOfSaleDetailsByIdQuery(int Id) : IRequest<Result<PosOfSaleDetailsDto, Error>>;

