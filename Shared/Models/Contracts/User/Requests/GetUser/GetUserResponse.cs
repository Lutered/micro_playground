using Shared.Models.DTOs.User;

namespace Shared.Models.Contracts.User.Requests.GetUser
{
    public class GetUserResponse
    {
        public UserDTO User { get; set; }
    }
}
