namespace Shared.Models.Common
{
    public class HandlerResult : BaseResult<object>
    {
        private HandlerResult(
            bool isSuccess,
            HandlerError error
         )
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static HandlerResult Success()
            => new HandlerResult(true, null);

        public static HandlerResult Failure(HandlerErrorType errorType, string message = "")
            => new HandlerResult(false, new HandlerError(message, errorType));
    }

    public class HandlerResult<T> : BaseResult<T>
    {
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

        public static HandlerResult<T> Failure(HandlerErrorType errorType, string message = "")
            => new HandlerResult<T>(false, default, new HandlerError(message, errorType));
    }
}
