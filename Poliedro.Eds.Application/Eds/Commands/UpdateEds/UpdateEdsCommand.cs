using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Eds.Commands.UpdateEds;

public record UpdateEdsCommand(int IdEds, string Name, string Nit, string Address, string Sicom, int IdBusiness) : IRequest<Result<VoidResult, Error>>;