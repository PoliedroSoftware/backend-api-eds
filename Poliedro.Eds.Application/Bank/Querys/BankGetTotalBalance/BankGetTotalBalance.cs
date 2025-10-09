using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;

namespace Poliedro.Eds.Application.Bank.Querys.BankGetTotalBalance;

public record BankGetTotalBalance(int IdAccount) : IRequest<BankDto?>;
