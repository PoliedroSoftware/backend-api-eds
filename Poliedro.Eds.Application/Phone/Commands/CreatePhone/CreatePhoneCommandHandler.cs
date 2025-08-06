using System.Net;
using AutoMapper;
using FluentValidation;
using MediatR;
using Poliedro.Eds.Application.Common.Helper.removekey;
using Poliedro.Eds.Application.Ports.Redis;
using Poliedro.Eds.Application.Phone.Errors;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Phone.DomainServices.Create;
using Poliedro.Eds.Domain.Phone.Entities;

namespace Poliedro.Eds.Application.Phone.Commands.CreatePhone;

public class CreatePhoneCommandHandler(
    IPhoneCreateService phoneCreateService,
    IMapper mapper,
    IValidator<CreatePhoneRequestDto> validator,
    IRedisService redisService) : IRequestHandler<CreatePhoneCommand, Result<VoidResult, Error>>
{
    public async Task<Result<VoidResult, Error>> Handle(CreatePhoneCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<VoidResult, Error>.Failure(
                Error.CreateInstance("ValidationFailed",
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)), 
                    HttpStatusCode.BadRequest));
        }

        var phoneEntities = request.Request.Numbers
            .Select(number => new PhoneEntity { Number = NormalizePhoneNumber(number) })
            .ToList();

        foreach (var phoneEntity in phoneEntities)
        {
            var result = await phoneCreateService.CreateAsync(phoneEntity);
            if (!result.IsSuccess)
            {
                return PhoneErrorBuilder.PhoneCreationException();
            }
        }

        await redisService.RemoveByPrefixAsync("phone:");
        return VoidResult.Instance;
    }

    private static string NormalizePhoneNumber(string number)
    {
        var digits = new string(number.Where(char.IsDigit).ToArray());

        if (digits.StartsWith("57") && digits.Length == 12)
            return digits;

        if (digits.StartsWith("0"))
            digits = digits.Substring(1);

        if (!digits.StartsWith("57"))
            digits = "57" + digits;

        return digits;
    }
}
