namespace Shared
{
    public class HandlerResult<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public HandlerError Error { get; }

        private HandlerResult(
            bool isSuccess, 
            T? value,
            HandlerError error
         ) 
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static HandlerResult<T> Success(T value)
            => new HandlerResult<T>(true, value, null);

        public static HandlerResult<T> Failure(HandlerError error)
            => new HandlerResult<T>(false, default, error);
    }

    public class HandlerError
    {
        public string Message { get; init; }
        public HandlerErrorType? Type { get; init; } 

        public HandlerError(string message, HandlerErrorType? type = null)
        {
            Message = message;
            Type = type;
        }
    }

    public enum HandlerErrorType
    {
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Internal
    }
}
