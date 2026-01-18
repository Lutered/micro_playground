using Microsoft.AspNetCore.Routing;

namespace Shared.Interfaces.Common
{
    public interface IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app);
    }
}
