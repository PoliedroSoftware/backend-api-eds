using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.Business.Commands.UpdateBusiness;

public record UpdateBusinessCommand : IRequest<Result<VoidResult, Error>>
{
    public int IdBusiness { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty; 
}
