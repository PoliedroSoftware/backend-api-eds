using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetById;

public record BankGetId(int Id) : IRequest<BankDto?>;
