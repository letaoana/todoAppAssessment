using Newtonsoft.Json;
using System;

namespace Todo.API.Client.Models
{
    public class AuthenticateUserResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires")]
        public DateTimeOffset Expires { get; set; }
    }
}
