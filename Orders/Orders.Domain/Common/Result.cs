namespace Orders.Domain.Common;

public class Result
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }

    public Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error);
    }
}


public class Result<T> : Result
{
    public T Value { get; private set; }
    public Result(bool isSuccess, string error, T value) : base(isSuccess, error)
    {
        Value = value;
    }
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, string.Empty, value);
    }
    public static new Result<T> Failure(string error)
    {
        return new Result<T>(false, error, default(T));
    }
}
