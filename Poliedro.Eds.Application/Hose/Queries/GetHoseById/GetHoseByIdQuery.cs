using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.Dtos;

namespace Poliedro.Eds.Application.Hose.Queries.GetHoseById;
public record GetHoseByIdQuery(int Id) : IRequest<Result<HoseDto, Error>>;
