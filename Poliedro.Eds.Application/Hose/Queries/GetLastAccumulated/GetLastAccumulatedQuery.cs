using MediatR;
using Poliedro.Eds.Application.Hose.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Hose.Queries.GetLastAccumulated;
public record GetLastAccumulatedQuery(int idDispenser, int idHose) : IRequest<Result<LastAccumulatedDto, Error>>;