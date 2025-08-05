namespace Shared
{
    public class HandlerResult<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public AppError Error { get; }

        private HandlerResult(
            bool isSuccess, 
            T? value,
            AppError error
         ) 
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static HandlerResult<T> Success(T value)
            => new HandlerResult<T>(true, value, null);

        public static HandlerResult<T> Failure(AppError error)
            => new HandlerResult<T>(false, default, error);
    }

    public class AppError
    {
        public string Message { get; init; }
        public ErrorType? Type { get; init; } 

        public AppError(string message, ErrorType? type = null)
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
