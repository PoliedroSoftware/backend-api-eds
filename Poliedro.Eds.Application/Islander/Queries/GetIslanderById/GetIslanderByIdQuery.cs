using MediatR;
using Poliedro.Eds.Application.Islander.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Islander.Queries.GetIslanderById;
    public record GetIslanderByIdQuery(int Id) : IRequest<Result<IslanderDto, Error>>;
    
