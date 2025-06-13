namespace Poliedro.Eds.Domain.Common.Results;

public class Result<TValue, TError>
{
    public readonly TValue? Value;
    public readonly TError? Error;
    public bool IsSuccess { get; }

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    private Result(TError error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

    public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(value);
    public static Result<TValue, TError> Failure(TError error) => new Result<TValue, TError>(error);


}

public class VoidResult
{
    /// <summary>
    /// Creates an instance of the VoidResult.
    /// </summary>
    /// <remarks>
    /// This method should be used exclusively for return a VoidResult on specif.
    /// </remarks>
    public static VoidResult Instance { get; } = new VoidResult();
}
