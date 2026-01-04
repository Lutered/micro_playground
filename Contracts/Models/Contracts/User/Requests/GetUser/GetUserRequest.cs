namespace Shared.Models.Contracts.User.Requests.GetUser
{
    public class GetUserRequest
    {
        public Guid UserId { get; set; }

        public GetUserRequest(Guid userId)
        {
            UserId = userId;
        }

    }
}
