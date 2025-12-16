using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using UsersAPI.DTOs;

namespace UsersAPI.Features.Queries.GetUser
{
    public class GetUserQuery : IRequest<HandlerResult<UserDTO>>
    {
        public Guid? Id { get; } = null;
        public string Username { get; } = string.Empty;

        public GetUserQuery(string username)
        {
            Username = username;
        }

        public GetUserQuery(Guid id)
        {
            Id = id;
        }
    }
}
