using MediatR;
using Poliedro.Eds.Application.ShoppingProduct.Commands.CreateShoppingProduct;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.CreateShoppingProductInventory
{
    public record CreateShoppingProductInventoryCommand(CreateShoppingProductInventoryRequestDto Request) : IRequest<Result<VoidResult, Error>>;
}
