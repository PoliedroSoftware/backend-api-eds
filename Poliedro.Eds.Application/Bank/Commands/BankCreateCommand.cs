using MediatR;
using Poliedro.Eds.Application.Bank.Dtos;

namespace Poliedro.Eds.Application.Bank.Commands;

public record BankCreateCommand(BankDtoCreateRequest Request) : IRequest<BankDto>;
