using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Day7.Services;

namespace Day7
{
    public class ImageSearch
    {
        private readonly IImageApiService _imageService;

        public ImageSearch(IImageApiService imageService)
        {
            _imageService = imageService;
        }

        [FunctionName("ImageSearch")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "photos")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string _query = req.Query["query"];

                // Count variable to be used later if more than one image is required
                int _count = int.TryParse(req.Query["count"], out _count) ? _count : 1;
                
                return _query != null
                    ? (ActionResult)new OkObjectResult(await _imageService.GetImage(_query).ConfigureAwait(false))
                    : new BadRequestObjectResult("Please pass a search query on the query string");
            }
            catch(Exception e)
            {
                log.LogError(e, e.Message, null);
                return new StatusCodeResult(500);
            }
        }
    }
}