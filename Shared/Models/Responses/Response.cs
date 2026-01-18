
namespace Shared.Models.Responses
{
    public class Response<T>
    {
        public T Data { get; private set; }

        public Response(T data)
        {
            Data = data;
        }
    }
}
