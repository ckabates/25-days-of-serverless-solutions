using Day3.Configurations;
using Day3.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3.Handlers
{
    public class GitPushHandler : IGitPushHandler
    {
        private readonly IOptions<SecretSantaCosmodDbConfiguration> _config;
        private JObject _json;
        private IEnumerable<string> _addedFiles;

        public GitPushHandler(IOptions<SecretSantaCosmodDbConfiguration> config)
        {
            _addedFiles = new List<string>();
            _config = config;
        }

        public async Task HandleRequest(string payload)
        {
            parsePayload(payload);
            checkForAddedFiles();
            await saveImageUrls();
        }

        private void parsePayload(string payload)
        {
            try
            {
                _json = JObject.Parse(payload);
            }
            catch
            {
                throw new Exception("Invalid Json");
            }
        }

        private void checkForAddedFiles()
        {
            if (isValidJArray(_json["commits"]))
            {
                _addedFiles = _json["commits"]
                    .SelectMany(c => isValidJArray(c["added"]) ? c["added"] : new JArray(), (commit, image) => new { commit, image })
                    .Where(commitAndImage => commitAndImage.image.Value<string>().ToLower().EndsWith(".png"))
                    .Select(commitAndImage => $"{commitAndImage.commit["url"].Value<string>()}/{commitAndImage.image}");
            }
        }

        private async Task saveImageUrls()
        {
            CosmosClientOptions _options = new CosmosClientOptions() { AllowBulkExecution = true };
            var _cosmosClient = new CosmosClient(_config.Value.ConnectionString, _options);
            Database _database = _cosmosClient.GetDatabase(_config.Value.DatabaseId);
            Container _container = await _database.CreateContainerIfNotExistsAsync(_config.Value.ContainerId, _config.Value.PartitionKey).ConfigureAwait(false);

            List<Task> _tasks = new List<Task>();
            foreach (string s in _addedFiles)
            {
                _tasks.Add(_container.CreateItemAsync<SecretSantaImage>(new SecretSantaImage(s)));
            }

            await Task.WhenAll(_tasks);
        }

        private bool isValidJArray(JToken array)
        {
            return array != null && array.HasValues;
        }
    }
}
