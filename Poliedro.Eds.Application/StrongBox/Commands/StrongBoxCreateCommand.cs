using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;

namespace Poliedro.Eds.Application.StrongBox.Commands
{
    public record StrongBoxCreateCommand(StrongBoxCreateRequest Request) : IRequest<StrongBoxDto>;
}
