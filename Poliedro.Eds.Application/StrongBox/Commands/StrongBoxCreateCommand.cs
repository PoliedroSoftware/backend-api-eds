using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.StrongBox.Commands;

public record StrongBoxCreateCommand(StrongBoxDtoCreateRequest Request) : IRequest<StrongBoxDto>;
