using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Day21.Core
{
    public static class ExtensionMethods
    {
        public static async Task<dynamic> DeserializeRequest(this HttpRequest req)
        {
            string _requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject(_requestBody);
        }
    }
}
