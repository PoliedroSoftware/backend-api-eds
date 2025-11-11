using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Compartiment.DomainCompartiment;
using Poliedro.Eds.Domain.Compartiment.Entities;

namespace Poliedro.Eds.Application.Compartiment.Commands.UpdateCompartiment
{
    public class UpdateCompartimentCommandHandler(
        ICompartimentUpdateService compartimentDomainCompartiment,
        IMapper mapper,
        IValidator<UpdateCompartimentCommand> validator
        ) : IRequestHandler<UpdateCompartimentCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateCompartimentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<VoidResult, Error>.Failure(
                        Error.CreateInstance("ValidationFailed", errors, HttpStatusCode.BadRequest));
                }

                var compartimentEntity = mapper.Map<CompartimentEntity>(request);
                var result = await compartimentDomainCompartiment.UpdateAsync(compartimentEntity);

                if (!result.IsSuccess)
                    return result.Error!;
                return result.Value!;
            }
            catch (ValidationException valEx)
            {
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("ValidationError", valEx.Message, HttpStatusCode.BadRequest));
            }
            catch (AutoMapperMappingException mapEx)
            {
                // Log the full exception server-side for debugging
                Console.WriteLine($"[ERROR] AutoMapper mapping failed: {mapEx}");
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("MappingError", "Failed to map the compartiment data. Please verify all required fields are provided correctly.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                // Log the full exception server-side for debugging
                Console.WriteLine($"[ERROR] Unexpected error in UpdateCompartimentCommandHandler: {ex}");
                return Result<VoidResult, Error>.Failure(
                    Error.CreateInstance("InternalError", "An unexpected error occurred while processing the update request. Please contact support if the problem persists.", HttpStatusCode.InternalServerError));
            }
        }
    }
}
