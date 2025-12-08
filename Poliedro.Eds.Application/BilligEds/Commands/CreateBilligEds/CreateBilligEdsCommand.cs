using MediatR;
using Poliedro.Eds.Domain.BilligEds.Entities;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.BilligEds.Commands.CreateBilligEds;

public record CreateBilligEdsCommand(BillidEdsRequestEntity Request) : IRequest<Result<VoidResult, Error>>;
