using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Poliedro.Eds.Application.RegisterShift.Dtos;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Common.Results;

namespace Poliedro.Eds.Application.RegisterShift.Commands.CreateRegisterShift;

public record CreateRegisterShiftCommand(RegisterShiftDto Request) : IRequest<Result<VoidResult, Error>>;
