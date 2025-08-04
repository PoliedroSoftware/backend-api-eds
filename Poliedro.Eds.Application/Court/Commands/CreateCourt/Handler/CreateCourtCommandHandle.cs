using System.Net;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;

namespace Poliedro.Eds.Application.Court.Commands.CreateCourt.Handler
{
    public class CreateCourtCommandHandle(IMapper mapper,
        ICourtDomainService courtDomainService,
        IGetProductAndCompartiment getProductAndCompartiment,
        IGetExpenditureId getExpenditureId,
        IGetTypeOfCollectionId getTypeOfCollectionId,
        IRedisService redisService,
        ICourtUpdateInventoryService courtUpdateInventoryService,
        IMediator mediator
        ) : IRequestHandler<CreateCourtCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCourtCommand request, CancellationToken cancellationToken)
        {
            var courtEntity = mapper.Map<CourtEntity>(request);

            var TotalAccumulatedAmount = GetTotalAccumulatedAmount(request);

            var TotalAccumulatedGallons = GetTotalAccumulatedGallons(request);

            var TotalAmount = GetTotalAmount(request);

            var TotalExpenditures = GetTotalExpenditures(request);

            var TotalTypeOfCollection = GetTotalTypeOfCollection(request);

            var TotalAmountCollection = GetTotalAmountCollection(request);

            if (TotalAmount != TotalTypeOfCollection)
            {
                throw new InvalidOperationException("Error, La suma de los tipos de cobro no coincide con el total del dia");
            }

            double cash = TotalTypeOfCollection - TotalExpenditures;
            if (cash < 0)
            {
                throw new InvalidOperationException("Error, El total de efectivo no puede ser negativo");
            }
            if (courtEntity.CourtExpenditures.Count() > 0)
            {
                foreach (var item in courtEntity.CourtExpenditures)
                {
                    var expenditureId = await getExpenditureId.GetExpenditureIdAsync(item.ExpenditureName);
                    item.IdExpenditures = (int)expenditureId;
                }
            }

            if (courtEntity.CourtTypeOfCollections.Count() > 0)
            {
                foreach (var item in courtEntity.CourtTypeOfCollections)
                {
                    var typeOfCollectionId = await getTypeOfCollectionId.GetTypeOfCollectionIdAsync(item.TypeOfCollectionName);
                    item.IdTypeOfCollection = (int)typeOfCollectionId;
                }
            }

            if (courtEntity.CourtTypeOfCollections.Count() > 0)
            {
                foreach (var item in courtEntity.CourtDispensers)
                {
                    ProductAndCompartimentEntity productAndCompartiment = await getProductAndCompartiment.GetProductAndCompartimentAsync(item.IdHose);
                    item.IdProduct = productAndCompartiment.IdProduct;
                    item.IdCompartiment = productAndCompartiment.IdCompartiment;
                }
            }

            courtEntity.CourtInventory = new InventoryEntity
            {
                Date = courtEntity.DateStarttime,
                ReferenceType = "court",
            };

            var result = await courtDomainService.CreateAsync(courtEntity);

            await RedisHelper.RemoveCacheIfSuccessAsync(result, redisService,
            KeyRedisConstants.BUSINESS,
            KeyRedisConstants.COMPARTIMENT,
            KeyRedisConstants.DISPENSERS,
            KeyRedisConstants.EDS,
            KeyRedisConstants.EXPENDITURES,
            KeyRedisConstants.HOSE,
            KeyRedisConstants.ISLANDER,
            KeyRedisConstants.PRODUCT,
            KeyRedisConstants.TRANSLATION,
            KeyRedisConstants.TYPE_OF_COLLECTION);

            if (!result.IsSuccess)
                return result.Error!;

            if (result.IsSuccess)
            {
                List<CourtDispenserSaleEntity> courtDispenserSaleEntities = [];
                CourtDispenserSaleEntity courtDispenserSaleEntity = new();
                foreach (var item in courtEntity.CourtDispensers)
                {
                    courtDispenserSaleEntity = mapper.Map<CourtDispenserSaleEntity>(item);
                    courtDispenserSaleEntities.Add(courtDispenserSaleEntity);
                    courtDispenserSaleEntities = courtDispenserSaleEntities
                        .Select((entity, index) =>
                        {
                            mapper.Map(request.CourtDispensers.ElementAt(index), entity);
                            return entity;
                        }).ToList();
                }
                var inventoryResult = await courtUpdateInventoryService.CourtUpdateInventoryAsync(courtDispenserSaleEntities);
                if (!inventoryResult.IsSuccess)
                    return inventoryResult;
            }


            

            if (result.IsSuccess)
            {
                var courtDto = mapper.Map<CourtDto>(courtEntity); 

                await mediator.Send(new SendWhatsAppMessageCommand
                {
                    PhoneNumber = "573182989981", 
                    Court = courtDto
                });
            }

            return result.Value!;
        }

        private double GetTotalAccumulatedAmount(CreateCourtCommand command)
        {
            return command.CourtDispensers.Sum(d => d.AmountDifferenceResult);
        }

        private double GetTotalAccumulatedGallons(CreateCourtCommand command)
        {
            return command.CourtDispensers.Sum(d => d.AccumulatedGallons);
        }

        private double GetTotalAmount(CreateCourtCommand command)
        {
            return command.CourtDispensers.Sum(d => d.AmountDifferenceResult);
        }

        private double GetTotalExpenditures(CreateCourtCommand command)
        {
            if (command.CourtExpenditures == null || !command.CourtExpenditures.Any())
            {
                return 0;
            }
            return command.CourtExpenditures.Sum(d => d.Amount);
        }

        private double GetTotalTypeOfCollection(CreateCourtCommand command)
        {
            return command.CourtTypeOfCollections.Sum(d => d.Amount);
        }

        private double GetTotalAmountCollection(CreateCourtCommand command)
        {
            return command.CourtTypeOfCollections.Sum(d => d.Amount);
        }
    }
}
