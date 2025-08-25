using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;

namespace Poliedro.Eds.Application.StrongBox.Commands;

public record StrongBoxCreateCommand(StrongBoxDtoCreateRequest Request) : IRequest<Unit>;
