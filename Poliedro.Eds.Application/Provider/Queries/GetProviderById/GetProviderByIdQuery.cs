using MediatR;
using Poliedro.Eds.Application.Provider.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Provider.Queries.GetProviderById;
public record GetProviderByIdQuery(int Id) : IRequest<Result<ProviderDto, Error>>;