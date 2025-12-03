using MediatR;
using Shared.Models.Common;
using UsersAPI.DTOs;

namespace UsersAPI.Features.Queries.GetUser
{
    public class GetUserQuery : IRequest<HandlerResult<AppUserDTO>>
    {
        public string Username { get; set; }

        public GetUserQuery(string username)
        {
            Username = username;
        }
    }
}
