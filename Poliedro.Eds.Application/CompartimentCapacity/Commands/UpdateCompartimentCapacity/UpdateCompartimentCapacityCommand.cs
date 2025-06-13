using MediatR;
using Poliedro.Eds.Application.CompartimentCapacity.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Application.CompartimentCapacity.Commands.UpdateCompartimentCapacity;
public record UpdateCompartimentCapacityCommand(int IdCompartimentCapacity,int IdCompartiment, int IdCapacity,byte Default) : IRequest<Result<VoidResult, Error>>;