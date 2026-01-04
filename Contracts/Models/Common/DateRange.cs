using Newtonsoft.Json;

namespace Shared.Models.Common
{
    public class DateRange
    {
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
    }
}
