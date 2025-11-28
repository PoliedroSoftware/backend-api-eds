using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Poliedro.Eds.Application.Hose.Commands.CreateHose;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;

namespace Poliedro.Eds.Application.PosOfSale.Commands.CreatePosOfSale;

public record CreatePosOfSaleCommand(CreatePosOfSaleRequestDto Request) : IRequest<Result<VoidResult, Error>>;

