using System.Net;

namespace AuthAPI.Mediator
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public AppError Error { get; }

        private Result(
            bool isSuccess, 
            T? value,
            AppError error
         ) 
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, value, null);

        public static Result<T> Failure(AppError error)
            => new Result<T>(false, default, error);
    }

    public class AppError
    {
        public string Message { get; init; }
        public ErrorType? Type { get; init; } 

        public AppError(string message, ErrorType? type)
        {
            Message = message;
            Type = type;
        }
    }

    public enum ErrorType
    {
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Internal
    }
}
