using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Day7.Models
{
    public class ImageSearchResponse
    {
        [JsonProperty("image_url")]
        public Uri Image { get; set; }
    }
}
