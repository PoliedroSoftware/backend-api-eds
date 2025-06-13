using MediatR;
using Poliedro.Eds.Application.EdsTank.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Application.EdsTank.Commands.UpdateEdsTank;
public record UpdateEdsTankCommand(int IdEdsTank,int IdEds,int IdTank) : IRequest<Result<VoidResult, Error>>;