using Newtonsoft.Json;

namespace Shared.Models.Common
{
    public class NumberRange
    {
        [JsonProperty("start")]
        public float Start { get; set; }

        [JsonProperty("end")]
        public float End { get; set; }
    }
}
