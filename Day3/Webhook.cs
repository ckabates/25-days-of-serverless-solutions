using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Day3.Handlers;

namespace Day3
{
    public class Webhook
    {
        private readonly IGitPushHandler _handler;

        public Webhook(IGitPushHandler handler)
        {
            _handler = handler;
        }

        [FunctionName("Webhook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GitPush")] HttpRequest req,
            ILogger log)
        {
            try
            {
                _handler.HandleRequest(await req.ReadAsStringAsync());

                log.LogInformation("Git push event processed.");
            }
            catch
            {
                log.LogError("Error processing git push event.");
            }

            return (ActionResult)new OkResult();
        }
    }
}
