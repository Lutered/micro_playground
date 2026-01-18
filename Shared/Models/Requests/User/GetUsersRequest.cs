using Microsoft.AspNetCore.Mvc;

namespace Shared.Models.Requests.User
{
    public class GetUsersRequest : PagedRequest
    {
        [FromQuery(Name = "username")]
        public string? Username { get; set; }

        [FromQuery(Name = "email")]
        public string? Email { get; set; }

        [FromQuery(Name = "startAge")]
        public float? StartAge { get; set; }

        [FromQuery(Name = "endAge")]
        public float? EndAge { get; set; }
    }
}
