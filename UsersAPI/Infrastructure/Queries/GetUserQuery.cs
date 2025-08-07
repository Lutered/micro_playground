using MediatR;
using Shared;
using UsersAPI.DTOs;

namespace UsersAPI.Infrastructure.Queries
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
