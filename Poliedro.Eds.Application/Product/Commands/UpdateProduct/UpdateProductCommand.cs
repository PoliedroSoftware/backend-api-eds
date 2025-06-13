using MediatR;
using Poliedro.Eds.Application.Product.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Application.Product.Commands.UpdateProduct;
public record UpdateProductCommand(int IdProduct,string Name,int IdProductType, double Price) : IRequest<Result<VoidResult, Error>>;