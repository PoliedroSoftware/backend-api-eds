using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Expenditures.Commands.CreateExpenditures;

public record CreateExpendituresCommand (CreateExpendituresRequestDto Request) : IRequest<Result<VoidResult, Error>>;
