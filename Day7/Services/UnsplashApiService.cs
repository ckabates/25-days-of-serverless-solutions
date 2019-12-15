using Day7.Configurations;
using Day7.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Day7.Services
{
    public class UnsplashApiService : IImageApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UnsplashConfiguration> _unsplashConfig;

        public UnsplashApiService(HttpClient httpClient, IOptions<UnsplashConfiguration> unsplashConfig)
        {
            _httpClient = httpClient;
            _unsplashConfig = unsplashConfig;
        }

        public async Task<ImageSearchResponse> GetRandomImage(string keywords)
        {
            HttpRequestMessage _request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_unsplashConfig.Value.ApiBaseUrl}photos/random?query={keywords}"),
                Headers =
                {
                    { "Authorization", $"Client-ID {_unsplashConfig.Value.AccessKey}" }
                }
            };

            var _response = await _httpClient.SendAsync(_request).ConfigureAwait(false);

            var _searchResult = JsonConvert
                .DeserializeObject<UnsplashImage>(
                    await _response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );

            return new ImageSearchResponse { Image = _searchResult?.GetImageUri() };
        }
    }
}
