using Microsoft.AspNetCore.Mvc;

namespace Shared.Models.Requests
{
    public class NumberRangeRequest
    {
        [FromQuery(Name = "start")]
        public float Start { get; set; }

        [FromQuery(Name = "end")]
        public float End { get; set; }
    }
}
