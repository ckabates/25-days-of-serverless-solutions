using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day7.Models
{
    public class UnsplashImage : IImage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("urls")]
        public Dictionary<string, string> Urls { get; set; }

        [JsonProperty("links")]
        public Dictionary<string, string> Links { get; set; }

        public Uri GetImageUri()
        {
            return new Uri(Links["download"] ?? Urls["regular"]);
        }
    }
}
