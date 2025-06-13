using MediatR;
using Poliedro.Eds.Application.ProductCompartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Application.ProductCompartiment.Commands.UpdateProductCompartiment;
public record UpdateProductCompartimentCommand(int IdProductCompartiment,int IdProduct,int IdCompartiment, double Stock) : IRequest<Result<VoidResult, Error>>;