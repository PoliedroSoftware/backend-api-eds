using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Compartiment.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using System.Net;

namespace Poliedro.Eds.Application.Compartiment.Queries.GetCompartimentById
{
    public class GetCompartimentByIdQueryHandler(
        ICompartimentGetByIdService compartimentDomainService,
        IMapper mapper,
        IValidator<GetCompartimentByIdQuery> validator)
        : IRequestHandler<GetCompartimentByIdQuery, Result<CompartimentDto, Error>>
    {
        public async Task<Result<CompartimentDto, Error>> Handle(GetCompartimentByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<CompartimentDto, Error>.Failure(
                Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
            }

            var result = await compartimentDomainService.GetByIdAsync(request.Id);
            if (!result.IsSuccess)
                return result.Error!;

            return mapper.Map<CompartimentDto>(result.Value);
        }
    }
}
