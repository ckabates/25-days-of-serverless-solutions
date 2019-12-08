using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day3.Models
{
    public class SecretSantaImage
    {
        public SecretSantaImage(string imageUrl)
        {
            Url = imageUrl;
            Id = Guid.NewGuid();
            Type = "png";
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
