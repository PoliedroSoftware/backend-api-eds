using System.ComponentModel.DataAnnotations;
using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment;

public record UpdateProductCompartimentCommand(int IdProductCompartiment, int IdProduct, int IdCompartiment, double Stock) : IRequest<Result<VoidResult, Error>>;
