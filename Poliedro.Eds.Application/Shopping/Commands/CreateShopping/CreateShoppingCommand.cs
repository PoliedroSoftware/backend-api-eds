using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Shopping.Commands.CreateShopping;

public record CreateShoppingCommand(CreateShoppingRequestDto Request) : IRequest<Result<VoidResult, Error>>;