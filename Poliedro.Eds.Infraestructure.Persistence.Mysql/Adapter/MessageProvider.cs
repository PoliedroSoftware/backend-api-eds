using Poliedro.Billing.Infraestructure.Persistence.Mysql;
using Poliedro.Eds.Domain.Ports;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Adapter;
public class MessageProvider : IMessageProvider
{
    public string ErrorValidatorFieldNotNull => MessageProviderResource.ErrorValidatorFieldNotNull;

    public string ErrorValidatorFieldNotEmpty => MessageProviderResource.ErrorValidatorFieldNotEmpty;

    public string ErrorValidatorFieldGreatherThanZero => MessageProviderResource.ErrorValidatorFieldGreatherThanZero;

    public string ErrorValidatorFieldLessThanTwelve => MessageProviderResource.ErrorValidatorFieldLessThanTwelve;

    public string ErrorValidatorResolutionTypeValid => MessageProviderResource.ErrorValidatorResolutionTypeValid;

    public string ErrorValidatorFieldPositive => MessageProviderResource.ErrorValidatorFieldPositive;

    public string ErrorValidatorFieldMustBeGreaterThanZero => MessageProviderResource.ErrorValidatorFieldMustBeGreaterThanZero;

    public string ErrorValidatorFieldMustBeNonNegative => MessageProviderResource.ErrorValidatorFieldMustBeNonNegative;

    public string ErrorValidatorFieldMustBePercentage => MessageProviderResource.ErrorValidatorFieldMustBePercentage;

    public string ErrorValidatorInvalidEmail => MessageProviderResource.ErrorValidatorInvalidEmail;

    public string ErrorValidatorFieldIsRequired => MessageProviderResource.ErrorValidatorFieldIsRequired;

    public string ErrorValidatorFieldMustBeGreaterThanOrEqualToZero => MessageProviderResource.ErrorValidatorFieldMustBeGreaterThanOrEqualToZero;



}

