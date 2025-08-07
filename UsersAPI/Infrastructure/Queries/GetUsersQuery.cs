using MediatR;
using Shared;
using UsersAPI.DTOs;
using UsersAPI.Helpers;

namespace UsersAPI.Infrastructure.Queries
{
    public class GetUsersQuery : IRequest<HandlerResult<PagedList<AppUserDTO>>>
    {
        public PageDTO PageParams { get; set; }
        public GetUsersQuery(PageDTO pageParams) 
        {
            PageParams = pageParams; 
        }
    }
}
