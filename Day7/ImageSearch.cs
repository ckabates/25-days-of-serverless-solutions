using Day7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                int _count = int.TryParse(req.Query["count"], out _count) ? _count : 1;

                var _images = await _imageService.GetImages(_query, _count).ConfigureAwait(false);

                OkObjectResult _result;

                if (_count == 1)
                {
                    _result = new OkObjectResult(_images.FirstOrDefault());
                }
                else
                {
                    _result = new OkObjectResult(_images);
                }

                return _query != null
                    ? (ActionResult)_result
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