using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Dtos;
using Poliedro.Eds.Application.ShoppingProductInventory.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Queries.GetShoppingProductInventoryById
{
    public record GetShoppingProductInventoryByIdQuery(int Id) : IRequest<Result<ShoppingProductInventoryDto, Error>>;

}
