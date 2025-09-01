using System.ComponentModel.DataAnnotations;
using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Product.Commands.UpdateProduct;

<<<<<<< HEAD
public record UpdateProductCommand(int IdProduct, string Name, int IdProductType, double SellPrice, double PurchasePrice,
    double Stock) : IRequest<Result<VoidResult, Error>>;
=======
public record UpdateProductCommand(int IdProduct, string Name, int IdProductType, double Price) : IRequest<Result<VoidResult, Error>>;
>>>>>>> New-service-StrongBox
