using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using Shared.Models.Requests.User;

namespace UsersAPI.Features.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<HandlerResult<PagedList<UserDTO>>>
    {
        public GetUsersRequest Input { get; set; }
        public GetUsersQuery(GetUsersRequest input) 
        {
            Input = input; 
        }
    }
}
