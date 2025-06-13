using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Application.ProductType.Commands.UpdateProductType;
public record UpdateProductTypeCommand(int IdProductType,string Description) : IRequest<Result<VoidResult, Error>>;