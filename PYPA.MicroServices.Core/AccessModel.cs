using Couchbase;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PYPA.MicroServices.Core
{
    [Serializable]
    public class AccessModel
    {
        [JsonProperty("_id")]
        public String Id { get; }
        [Required]
        [JsonProperty("ip")]
        public string IP { get; set; }
        [Required]
        [JsonProperty("url")]
        public string URL { get; set; }
        [JsonProperty("params")]
        public string Params { get; set; }
        [Required]
        [JsonProperty("browser")]
        public string Browser { get; set; }
        [JsonProperty("date")]
        public Int64 Date { get; set; }
        [JsonProperty("type")]
        public string Type => "access";


        public AccessModel()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Date = DateTime.Now.Ticks;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Document<AccessModel> Document()
        {
            return new Document<AccessModel>
            {
                Id = Id,
                Content = this
            };
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(IP) &&
                   !string.IsNullOrEmpty(URL) &&
                   !string.IsNullOrEmpty(Browser);
        }
    }
}
