using System.Net;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Poliedro.Eds.Application.Common.Constants;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Application.Court.Services;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.StrongBox.Commands;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.Common.Enums;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;
using Poliedro.Eds.Domain.Inventory.Entities;

namespace Poliedro.Eds.Application.Court.Commands.CreateCourt.Handler
{
    public class CreateCourtCommandHandle(IMapper mapper,
        IGetProductAndCompartiment getProductAndCompartiment,
        IGetExpenditureId getExpenditureId,
        IGetTypeOfCollectionId getTypeOfCollectionId,
        ICourtTransactionalService courtTransactionalService,
        ILogger<CreateCourtCommandHandle> logger,
        IMediator mediator
        ) : IRequestHandler<CreateCourtCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateCourtCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("=== INICIANDO PROCESO DE CREACIÓN DE CORTE ===");
                
                // 1. Validaciones iniciales y cálculos
                var validationResult = await ValidateCourtDataAsync(request);
                if (!validationResult.IsSuccess)
                    return validationResult;

                // 2. Mapear entidad del corte
                var courtEntity = mapper.Map<CourtEntity>(request);

                // 3. Enriquecer datos de expenditures
                await EnrichExpendituresAsync(courtEntity);

                // 4. Enriquecer datos de tipos de cobro
                await EnrichTypeOfCollectionsAsync(courtEntity);

                // 5. Enriquecer datos de dispensadores
                await EnrichDispensersAsync(courtEntity);

                // 6. Preparar entidad de inventario
                courtEntity.CourtInventory = new InventoryEntity
                {
                    Date = courtEntity.DateStarttime,
                    ReferenceType = ReferenceType.Court,
                };

                // 7. Preparar entidades de venta de dispensadores
                var courtDispenserSaleEntities = PrepareCourtDispenserSaleEntities(courtEntity, request);

                // 8. Preparar datos de transacción de dispensadores
                var courtDispenserTransactionData = MapToTransactionData(request.CourtDispensers);

                // 9. Ejecutar transacción completa (incluye validación de precios, creación del corte e inventario)
                var transactionResult = await courtTransactionalService.ExecuteCourtTransactionWithPriceValidationAsync(
                    courtEntity,
                    courtDispenserTransactionData,
                    courtDispenserSaleEntities,
                    cancellationToken);

                if (!transactionResult.IsSuccess)
                {
                    logger.LogError("❌ TRANSACCIÓN FALLIDA: {ErrorDescription}", transactionResult.Error?.Description);
                    return transactionResult.Error!;
                }

                logger.LogInformation("✅ TRANSACCIÓN EXITOSA - INICIANDO PROCESOS POST-TRANSACCIÓN");

                // 10. Procesos post-transacción (estos no afectan la consistencia de datos)
                var courtDto = mapper.Map<CourtDto>(transactionResult.Value!);
                await ExecutePostTransactionProcessesAsync(courtDto, request, courtEntity, cancellationToken);

                logger.LogInformation("✅ PROCESO DE CORTE COMPLETADO EXITOSAMENTE");
                logger.LogInformation("===============================================");

                return Result<VoidResult, Error>.Success(VoidResult.Instance);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ ERROR CRÍTICO EN EL PROCESO DE CORTE: {ErrorMessage}", ex.Message);
                logger.LogInformation("===============================================");
                return Error.Internal("CourtProcessError", $"Error crítico durante el proceso de corte: {ex.Message}");
            }
        }

        private async Task<Result<VoidResult, Error>> ValidateCourtDataAsync(CreateCourtCommand request)
        {
            var TotalAmount = GetTotalAmount(request);
            var TotalTypeOfCollection = GetTotalTypeOfCollection(request);
            var TotalExpenditures = GetTotalExpenditures(request);

            if (Math.Abs(TotalAmount - TotalTypeOfCollection) > 0.01)
            {
                return Error.BadRequest("AmountMismatch", 
                    "Error, La suma de los tipos de cobro no coincide con el total del día");
            }

            double cash = TotalTypeOfCollection - TotalExpenditures;
            if (cash < 0)
            {
                return Error.BadRequest("NegativeCash", 
                    "Error, El total de efectivo no puede ser negativo");
            }

            logger.LogInformation("✅ Validaciones pasadas - Total: ${TotalAmount:N2}, Efectivo: ${Cash:N2}", TotalAmount, cash);
            return Result<VoidResult, Error>.Success(VoidResult.Instance);
        }

        private async Task EnrichExpendituresAsync(CourtEntity courtEntity)
        {
            if (courtEntity.CourtExpenditures?.Any() == true)
            {
                foreach (var item in courtEntity.CourtExpenditures)
                {
                    var expenditureId = await getExpenditureId.GetExpenditureIdAsync(item.ExpenditureName);
                    item.IdExpenditures = (int)expenditureId!;
                }
                logger.LogInformation("✅ {ExpenditureCount} gastos enriquecidos", courtEntity.CourtExpenditures.Count());
            }
        }

        private async Task EnrichTypeOfCollectionsAsync(CourtEntity courtEntity)
        {
            if (courtEntity.CourtTypeOfCollections?.Any() == true)
            {
                foreach (var item in courtEntity.CourtTypeOfCollections)
                {
                    var typeOfCollectionId = await getTypeOfCollectionId.GetTypeOfCollectionIdAsync(item.TypeOfCollectionName);
                    item.IdTypeOfCollection = (int)typeOfCollectionId!;
                }
                logger.LogInformation("✅ {CollectionCount} tipos de cobro enriquecidos", courtEntity.CourtTypeOfCollections.Count());
            }
        }

        private async Task EnrichDispensersAsync(CourtEntity courtEntity)
        {
            if (courtEntity.CourtDispensers?.Any() == true)
            {
                foreach (var item in courtEntity.CourtDispensers)
                {
                    var productAndCompartiment = await getProductAndCompartiment.GetProductAndCompartimentAsync(item.IdHose);
                    item.IdProduct = productAndCompartiment.IdProduct;
                    item.IdCompartiment = productAndCompartiment.IdCompartiment;
                }
                logger.LogInformation("✅ {DispenserCount} dispensadores enriquecidos", courtEntity.CourtDispensers.Count());
            }
        }

        private List<CourtDispenserSaleEntity> PrepareCourtDispenserSaleEntities(CourtEntity courtEntity, CreateCourtCommand request)
        {
            var courtDispenserSaleEntities = new List<CourtDispenserSaleEntity>();
            
            foreach (var item in courtEntity.CourtDispensers)
            {
                var courtDispenserSaleEntity = mapper.Map<CourtDispenserSaleEntity>(item);
                courtDispenserSaleEntities.Add(courtDispenserSaleEntity);
            }

            // Mapear datos adicionales desde el request
            for (int i = 0; i < courtDispenserSaleEntities.Count && i < request.CourtDispensers.Count(); i++)
            {
                mapper.Map(request.CourtDispensers.ElementAt(i), courtDispenserSaleEntities[i]);
            }

            logger.LogInformation("✅ {SaleEntityCount} entidades de venta preparadas", courtDispenserSaleEntities.Count);
            return courtDispenserSaleEntities;
        }

        private List<CourtDispenserTransactionData> MapToTransactionData(IEnumerable<CourtDispenserCommand> courtDispensers)
        {
            return courtDispensers.Select(cd => new CourtDispenserTransactionData
            {
                AccumulatedAmount = Math.Round(cd.AccumulatedAmount, 2),
                AccumulatedGallons = Math.Round(cd.AccumulatedGallons, 3),
                LastAccumulatedAmount = Math.Round(cd.LastAccumulatedAmount, 2),
                LastAccumulatedGallons = Math.Round(cd.LastAccumulatedGallons, 3),
                AmountDifferenceResult = Math.Round(cd.AmountDifferenceResult, 2),
                GallonsDifferenceResult = Math.Round(cd.GallonsDifferenceResult, 3),
                IdHose = cd.IdHose,
                NumberName = cd.NumberName,
                DispenserNumber = cd.DispenserNumber
            }).ToList();
        }

        private async Task ExecutePostTransactionProcessesAsync(CourtDto courtDto, CreateCourtCommand request, CourtEntity courtEntity, CancellationToken cancellationToken)
        {
            try
            {
                // Enriquecer DTO para WhatsApp
                if (courtDto.CourtDispensers != null && request.CourtDispensers != null)
                {
                    var courtDispensersList = courtDto.CourtDispensers.ToList();
                    var requestDispensersList = request.CourtDispensers.ToList();

                    for (int i = 0; i < courtDispensersList.Count && i < requestDispensersList.Count; i++)
                    {
                        courtDispensersList[i].AmountDifferenceResult = requestDispensersList[i].AmountDifferenceResult;
                        courtDispensersList[i].GallonsDifferenceResult = requestDispensersList[i].GallonsDifferenceResult;
                    }

                    courtDto.CourtDispensers = courtDispensersList;
                }

                // Enviar mensaje de WhatsApp
                await mediator.Send(new SendWhatsAppMessageCommand
                {
                    PhoneNumber = "573182989981", 
                    Court = courtDto
                }, cancellationToken);

                logger.LogInformation("✅ Mensaje de WhatsApp enviado");

                // Crear entrada en StrongBox si hay efectivo
                var money = GetCashOnly(request);
                if (money > 0)
                {
                    await mediator.Send(
                        new StrongBoxCreateCommand(new StrongBoxDtoCreateRequest
                        {
                            IdCorte = courtEntity.IdCourt,
                            Type = "CORTE",
                            Ammount = money,
                            Note = $"Corte #{courtEntity.IdCourt} Dinero en efectivo para la Caja!! ${money:N2} ",
                            IdEds  = courtEntity.IdEds
                        }),
                        cancellationToken
                    );
                    logger.LogInformation("✅ Entrada en StrongBox creada: ${Money:N2}", money);
                }
            }
            catch (Exception ex)
            {
                // Los errores en procesos post-transacción no deben afectar el resultado principal
                logger.LogWarning(ex, "⚠️ Error en proceso post-transacción (no crítico): {ErrorMessage}", ex.Message);
            }
        }

        // Métodos de cálculo privados (con redondeo para mayor precisión)
        private double GetTotalAccumulatedAmount(CreateCourtCommand command)
        {
            return Math.Round(command.CourtDispensers.Sum(d => d.AmountDifferenceResult), 2);
        }

        private double GetTotalAccumulatedGallons(CreateCourtCommand command)
        {
            return Math.Round(command.CourtDispensers.Sum(d => d.AccumulatedGallons), 3);
        }

        private double GetTotalAmount(CreateCourtCommand command)
        {
            return Math.Round(command.CourtDispensers.Sum(d => d.AmountDifferenceResult), 2);
        }

        private double GetTotalExpenditures(CreateCourtCommand command)
        {
            if (command.CourtExpenditures == null || !command.CourtExpenditures.Any())
            {
                return 0;
            }
            return Math.Round(command.CourtExpenditures.Sum(d => d.Amount), 2);
        }

        private double GetTotalTypeOfCollection(CreateCourtCommand command)
        {
            return Math.Round(command.CourtTypeOfCollections.Sum(d => d.Amount), 2);
        }

        private double GetTotalAmountCollection(CreateCourtCommand command)
        {
            return Math.Round(command.CourtTypeOfCollections.Sum(d => d.Amount), 2);
        }

        private double GetCashOnly(CreateCourtCommand command)
        {
            if (command.CourtTypeOfCollections is null) return 0;
            double Expenditures = 0;
            var efectivo = command.CourtTypeOfCollections
                .Where(t => string.Equals(t.TypeOfCollectionName, "EFECTIVO", StringComparison.OrdinalIgnoreCase))
                .Sum(t => (double)t.Amount);
            if (command.CourtExpenditures?.Count() > 0)
            {
                Expenditures = command.CourtExpenditures
                                .Sum(t => (double)t.Amount);
            }
           
            return Math.Round(efectivo - Expenditures, 2);
        }
    }
}
