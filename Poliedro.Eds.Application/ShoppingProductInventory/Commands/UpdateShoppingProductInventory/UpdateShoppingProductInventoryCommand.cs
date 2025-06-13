using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliedro.Eds.Application.ShoppingProductInventory.Commands.UpdateShoppingProductInventory
{
    public record UpdateShoppingProductInventoryCommand(
      int IdShopping,
      int IdInventory,
      int IdShoppingProduct
    ) : IRequest<Result<VoidResult, Error>>;
}
