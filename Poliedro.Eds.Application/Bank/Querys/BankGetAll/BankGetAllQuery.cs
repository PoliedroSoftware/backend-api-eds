using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetAll;

public record BankGetAllQuery(int? IdAccount = null, int? IdEds = null) : IRequest<IEnumerable<BankDto>>;
