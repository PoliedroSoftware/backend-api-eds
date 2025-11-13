using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Court.Dtos;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.FileUploadS3.Ports;

namespace Poliedro.Eds.Application.Court.Queris.GetCourtById;

public class GetCourtByIdQueryHandler(
        ICourtGetByIdDomainService courtGetByIdDomainService,
        IMapper mapper,
        IValidator<GetCourtByIdQuery> validator,
        IS3UrlGenerator s3UrlGenerator
    ) : IRequestHandler<GetCourtByIdQuery, Result<CourtDto, Error>>

{
    public async Task<Result<CourtDto, Error>> Handle(GetCourtByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result<CourtDto, Error>.Failure(
            Error.CreateInstance("ValidationFailed", validationResult.Errors.ToString(), HttpStatusCode.BadRequest));
        }

        var result = await courtGetByIdDomainService.GetByIdAsync(request.Id);
        if (!result.IsSuccess)
            return result.Error!;

        var courtValueMapper = mapper.Map<CourtDto>(result.Value);
        
        // Generar URLs pre-firmadas para los documentos
        if (courtValueMapper.CourtDocuments != null && courtValueMapper.CourtDocuments.Any())
        {
            foreach (var document in courtValueMapper.CourtDocuments.Where(d => d != null))
            {
                if (!string.IsNullOrWhiteSpace(document.DocumentName))
                {
                    // Generar URL pre-firmada (expira en 24 horas)
                    document.DocumentName = await s3UrlGenerator.GeneratePresignedUrlAsync(
                        document.DocumentName, 
                        expirationMinutes: 1440);
                }
            }
        }
        
        return courtValueMapper;
    }
}
