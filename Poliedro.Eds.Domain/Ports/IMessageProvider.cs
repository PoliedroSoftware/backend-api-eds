namespace Poliedro.Eds.Domain.Ports;

public interface IMessageProvider
{
    string ErrorValidatorFieldNotNull { get; }
    string ErrorValidatorFieldNotEmpty { get; }
    string ErrorValidatorFieldGreatherThanZero { get; }
    string ErrorValidatorFieldLessThanTwelve { get; }
    string ErrorValidatorResolutionTypeValid { get; }
    string ErrorValidatorFieldPositive { get; }
    string ErrorValidatorFieldMustBeGreaterThanZero { get; }
    string ErrorValidatorFieldMustBeNonNegative { get; }
    string ErrorValidatorFieldMustBePercentage { get; }
    string ErrorValidatorInvalidEmail { get; }
    string ErrorValidatorFieldMustBeGreaterThanOrEqualToZero { get; }
    string ErrorValidatorFieldIsRequired { get; }
}

