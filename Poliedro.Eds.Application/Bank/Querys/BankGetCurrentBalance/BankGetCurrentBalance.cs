using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetCurrentBalance;

public record BankGetCurrentBalance(int IdAccount) : IRequest<double>;
