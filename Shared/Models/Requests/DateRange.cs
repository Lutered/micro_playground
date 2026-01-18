using Microsoft.AspNetCore.Mvc;

namespace Shared.Models.Requests
{
    public class DateRangeRequest
    {
        [FromQuery(Name = "start")]
        public DateTime StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime EndDate { get; set; }
    }
}
