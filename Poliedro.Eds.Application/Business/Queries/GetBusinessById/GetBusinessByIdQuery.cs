using MediatR;
using Poliedro.Eds.Application.Business.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Queries.GetBusinessById;
public record GetBusinessByIdQuery (int Id) : IRequest<Result<BusinessDto, Error>>;