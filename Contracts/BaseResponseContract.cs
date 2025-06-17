namespace Contracts
{
    public class BaseResponseContract
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
