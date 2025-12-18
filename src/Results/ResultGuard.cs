using NDB.Abstraction.Results;

namespace NDB.Kit.Results;

/// <summary>
/// Guard helpers for Result flow control.
/// </summary>
public static class ResultGuard
{
    public static Result NotFoundIfNull(
        object? value,
        string message)
    {
        return value == null
            ? Result.Fail(ResultStatus.NotFound, message)
            : Result.Ok();
    }

    public static Result FailIf(
        bool condition,
        ResultStatus status,
        string message)
    {
        return condition
            ? Result.Fail(status, message)
            : Result.Ok();
    }
}
