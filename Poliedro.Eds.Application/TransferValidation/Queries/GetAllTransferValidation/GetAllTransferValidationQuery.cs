using MediatR;
using Poliedro.Eds.Application.TransferValidation.Dtos;
using Poliedro.Eds.Domain.Common.Pagination;

namespace Poliedro.Eds.Application.TransferValidation.Queries.GetAllTransferValidation;

public record GetAllTransferValidationQuery(PaginationParams PaginationParams) : IRequest<IEnumerable<TransferValidationDto>>;
