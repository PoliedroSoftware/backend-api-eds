using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.CreateBusiness;

public record CreateBusinessCommand(CreateBusinessRequestDto Request) : IRequest<Result<VoidResult, Error>>;