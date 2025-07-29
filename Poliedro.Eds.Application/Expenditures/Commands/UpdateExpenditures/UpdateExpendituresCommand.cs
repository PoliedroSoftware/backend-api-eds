using System.ComponentModel.DataAnnotations;
using MediatR;
using Poliedro.Eds.Application.Expenditures.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Expenditures.Commands.UpdateExpenditures;

public record UpdateExpendituresCommand(int IdExpenditures, string Description) : IRequest<Result<VoidResult, Error>>;
