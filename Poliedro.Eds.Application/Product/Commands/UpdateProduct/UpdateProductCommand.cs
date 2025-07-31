using System.ComponentModel.DataAnnotations;
using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Product.Commands.UpdateProduct;

public record UpdateProductCommand(int IdProduct, string Name, int IdProductType, double Price) : IRequest<Result<VoidResult, Error>>;
