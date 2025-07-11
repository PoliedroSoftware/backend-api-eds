using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Dto;
using Poliedro.Eds.Domain.Court.Entities;
using System.Text.Json;

namespace Poliedro.Eds.Application.Court.Commands.CreateCourt.Handler
{
    public class CreateCourtCommandHandle(IMapper mapper,
        ICourtDomainService courtDomainService, 
        IGetProductAndCompartiment  getProductAndCompartiment,
        IGetExpenditureId getExpenditureId,
        IGetTypeOfCollectionId getTypeOfCollectionId,
        ICourtUpdateInventoryService courtUpdateInventoryService) : IRequestHandler<CreateCourtCommand, Result<VoidResult, Error>>
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
            if(courtEntity.CourtExpenditures.Count() > 0)
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

            if (courtEntity.CourtTypeOfCollections.Count() > 0) {
                foreach (var item in courtEntity.CourtDispensers)
                {
                    ProductAndCompartimentDto productAndCompartiment = await getProductAndCompartiment.GetProductAndCompartimentAsync(item.IdHose);
                    item.IdProduct = productAndCompartiment.IdProduct;
                    item.IdCompartiment = productAndCompartiment.IdCompartiment;
                }
            }


            var result = await courtDomainService.CreateAsync(courtEntity);
            if (!result.IsSuccess)
                return result.Error!;
            if (result.IsSuccess)
            {
                List<ICourtDispenserSaleEntity> courtDispenserSaleEntities = [];
                ICourtDispenserSaleEntity courtDispenserSaleEntity = new();
                foreach (var item in courtEntity.CourtDispensers)
                {
                    courtDispenserSaleEntity = mapper.Map<ICourtDispenserSaleEntity>(item);
                    courtDispenserSaleEntities.Add(courtDispenserSaleEntity);
                    courtDispenserSaleEntities = courtDispenserSaleEntities
                        .Select((entity, index) =>
                        {

                            mapper.Map(request.CourtDispensers.ElementAt(index), entity);
                            return entity;
                        }).ToList();

                }

                await courtUpdateInventoryService.CourtUpdateInventoryAsync(courtDispenserSaleEntities);
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
