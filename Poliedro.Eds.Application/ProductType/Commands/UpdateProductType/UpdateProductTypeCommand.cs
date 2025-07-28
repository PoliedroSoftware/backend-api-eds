using System.ComponentModel.DataAnnotations;
using MediatR;
using Poliedro.Eds.Application.ProductType.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ProductType.Commands.UpdateProductType;

public record UpdateProductTypeCommand(int IdProductType, string Description) : IRequest<Result<VoidResult, Error>>;
