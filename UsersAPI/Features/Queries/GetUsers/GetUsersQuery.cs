using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests;
using UsersAPI.DTOs;
using UsersAPI.Helpers;

namespace UsersAPI.Features.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<HandlerResult<PagedList<AppUserDTO>>>
    {
        public PagedRequest PageParams { get; set; }
        public GetUsersQuery(PagedRequest pageParams) 
        {
            PageParams = pageParams; 
        }
    }
}
