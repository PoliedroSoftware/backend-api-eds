using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Shopping.Commands.UpdateShopping;

    public record UpdateShoppingCommand (
    int IdShopping,
    string Invoice,
    DateTime Date,
    int IdProvider,
    int IdCategory,
    double Amount) : IRequest<Result<VoidResult, Error>>;
