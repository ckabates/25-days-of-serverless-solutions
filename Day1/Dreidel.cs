using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Day1
{
    public static class Dreidel
    {
        [FunctionName("Dreidel")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "spin-the-dreidel")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var results = new string[] { "נ (Nun)", "ג (Gimmel)", "ה (Hay)", "ש (Shin)" };
                var random = new Random();
                var result = results[random.Next(4)];

                return (ActionResult)new OkObjectResult(result);
            }
            catch
            {
                return (ActionResult)new ObjectResult("The Dreidel is still spinning. Please try again later.") { StatusCode = 500 };
            }
        }
    }
}
