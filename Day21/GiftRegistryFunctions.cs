using Day21.Configurations;
using Day21.Core;
using Day21.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Day21
{
    public class GiftRegistryFunctions
    {
        private readonly IOptions<GiftRegistryConfiguration> _config;

        public GiftRegistryFunctions(IOptions<GiftRegistryConfiguration> config)
        {
            _config = config;
        }

        [FunctionName("GiftRegistry")]
        public Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<GiftRegistry>();

        [FunctionName("Open")]
        public async Task<IActionResult> Open(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "open")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            [DurableClient] IDurableEntityClient client,
            ILogger log)
        {
            try
            {
                Guid _id = Guid.NewGuid();

                TimeSpan timeout = TimeSpan.FromMinutes(_config.Value.ClosureDuration);
                DateTime deadline = DateTime.UtcNow.Add(timeout);

                await client.SignalEntityAsync<IGiftRegistry>(_id.ToString(), gr => gr.Create());
                await client.SignalEntityAsync<IGiftRegistry>(_id.ToString(), deadline, gr => gr.Close());

                return new OkObjectResult(_id);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Add")]
        public async Task<IActionResult> RunAdd(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "add")] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
        {
            try
            {
                dynamic _data = await req.DeserializeRequest();
                string _item = _data?.item ?? String.Empty;
                string _id = _data?.id ?? String.Empty;

                if (String.IsNullOrEmpty(_item) || String.IsNullOrEmpty(_id))
                {
                    return new BadRequestResult();
                }

                var _registry = 
                    await client.ReadEntityStateAsync<GiftRegistry>(new EntityId(nameof(GiftRegistry), _id));

                if (!_registry.EntityState.IsOpen)
                {
                    return new BadRequestObjectResult("Cannot add itms to a closed gift registry");
                }

                await client.SignalEntityAsync<IGiftRegistry>(_id.ToString(), gr => gr.Add(_item));

                return new OkResult();
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Peek")]
        public async Task<IActionResult> RunPeek(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "peek")] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
        {
            try
            {
                dynamic _data = await req.DeserializeRequest();
                string _id = _data?.id ?? String.Empty;

                if (String.IsNullOrEmpty(_id))
                {
                    return new BadRequestResult();
                }

                var _registry = 
                    await client.ReadEntityStateAsync<GiftRegistry>(new EntityId(nameof(GiftRegistry), _id));

                return new OkObjectResult(_registry.EntityState);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Finish")]
        public async Task<IActionResult> RunFinish(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "finish")] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
        {
            try
            {
                dynamic _data = await req.DeserializeRequest();
                string _id = _data?.id ?? String.Empty;

                if (String.IsNullOrEmpty(_id))
                {
                    return new BadRequestResult();
                }

                await client.SignalEntityAsync<IGiftRegistry>(_id, gr => gr.Close());

                return new OkResult();
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Stats")]
        public async Task<IActionResult> RunStats(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "stats")] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
        {
            try
            {
                var _query = new EntityQuery
                {
                    EntityName = nameof(GiftRegistry),
                    FetchState = true
                };

                var _result = await client.ListEntitiesAsync(_query, CancellationToken.None);

                var _statistics = new GiftRegistryStatistics
                {
                    TotalRegistries = _result.Entities.Count(),
                    TotalItems = _result.Entities.Sum(e => e.State["Items"].Count())
                };

                return new OkObjectResult(_statistics);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
