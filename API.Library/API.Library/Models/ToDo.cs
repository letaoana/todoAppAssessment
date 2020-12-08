using Newtonsoft.Json;
using System;

namespace Todo.API.Client.Models
{
    public class ToDo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
