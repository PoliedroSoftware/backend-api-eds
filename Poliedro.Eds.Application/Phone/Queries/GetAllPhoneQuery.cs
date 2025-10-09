using MediatR;
using Poliedro.Eds.Application.Phone.Dtos;
using Poliedro.Eds.Domain;

namespace Poliedro.Eds.Application.Phone.Queries.GetAllPhones;

public record GetAllPhonesQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<PhoneDto>>;
