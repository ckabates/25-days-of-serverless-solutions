﻿using Day7.Configurations;
using Day7.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Day7.Services
{
    public class UnsplashApiService : IImageApiService
    {
        private const int MAX_COUNT = 30;
        private readonly HttpClient _httpClient;
        private readonly IOptions<UnsplashConfiguration> _unsplashConfig;

        public UnsplashApiService(HttpClient httpClient, IOptions<UnsplashConfiguration> unsplashConfig)
        {
            _httpClient = httpClient;
            _unsplashConfig = unsplashConfig;
        }

        public async Task<ImageSearchResponse> GetImage(string keywords)
        {
            var _response = await getImages(keywords);

            var _searchResult = JsonConvert
                .DeserializeObject<UnsplashImage>(
                    await _response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );

            return new ImageSearchResponse { Image = _searchResult?.GetImageUri() };
        }

        public async Task<IEnumerable<ImageSearchResponse>> GetImages(string keywords, int count)
        {
            var _response = await getImages(keywords, count);

            var _images = JsonConvert
                .DeserializeObject<List<UnsplashImage>>(
                    await _response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );

            return _images.Select(i => new ImageSearchResponse { Image = i.GetImageUri() });
        }

        private async Task<HttpResponseMessage> getImages(string keywords, int count = 1)
        {
            var _requestUri = $"{_unsplashConfig.Value.ApiBaseUrl}photos/random?query={keywords}";

            if (count > 1)
                _requestUri = $"{_requestUri}&count={Math.Max(count, MAX_COUNT)}";

            HttpRequestMessage _request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_requestUri),
                Headers =
                {
                    { "Authorization", $"Client-ID {_unsplashConfig.Value.AccessKey}" }
                }
            };

            return await _httpClient.SendAsync(_request).ConfigureAwait(false);
        }
    }
}
