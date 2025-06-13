using MediatR;
using Poliedro.Eds.Application.HoseHistory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.HoseHistory.Queries.GetHoseHistoryById;

public record GetHoseHistoryByIdQuery(int Id) : IRequest<Result<HoseHistoryDto, Error>>;
