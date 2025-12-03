using MediatR;
using Shared.Models.Common;
using UsersAPI.DTOs;
using UsersAPI.Helpers;

namespace UsersAPI.Features.Queries.GetUsers
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
