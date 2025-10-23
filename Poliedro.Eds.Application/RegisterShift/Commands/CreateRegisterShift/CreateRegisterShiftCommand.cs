using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Poliedro.Eds.Application.RegisterShift.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Poliedro.Eds.Application.RegisterShift.Commands.CreateRegisterShift;

public record CreateRegisterShiftCommand(RegisterShiftDto Request) : IRequest<RegisterShiftDto>;
