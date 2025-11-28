using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Hose.Dtos;
using Poliedro.Eds.Domain.PosOfSale.Dtos;

namespace Poliedro.Eds.Application.PosOfSale.Queries.GetPostOfSaleById;

public record GetPosOfSaleByIdQuery(int Id) : IRequest<Result<PosOfSaleDto, Error>>;

