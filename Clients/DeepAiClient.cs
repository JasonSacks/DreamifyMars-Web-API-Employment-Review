using DreamInMars.Configuration;
using DreamInMars.Dto;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DreamInMars.Client
{
    public class DeepAiClient : IDeepAiClient
    {
        private class DeepDreamResult
        {
            public string output_url { get; set; }
        }

        private readonly HttpClient _client;
        private readonly DeepDreamConfiguration _configuration;

        public DeepAiClient(HttpClient client, IOptions<DeepDreamConfiguration> configuration)
        {
            _client = client;
            _configuration = configuration.Value;
        }

        private async Task<Stream> GetImageAsync(string path)
        {
            var result = await _client.GetAsync(path);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> ProcessDeepAiImageAsync(Image image)
        {
            var stream = await GetImageAsync(image.Path);
            return await ProcessDeepAiImageAsync(stream);
        }

        public async Task<Stream> ProcessDeepAiImageAsync(Stream stream)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(stream), "image", "upload.jpg");

            var request = new HttpRequestMessage(HttpMethod.Post, _configuration.ApiPath)
            {
                Content = formData
            };

            request.Headers.Add("Api-Key", _configuration.ApiKey);
            request.Method = HttpMethod.Post;

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var deepResult = JsonSerializer.Deserialize<DeepDreamResult>(await response.Content.ReadAsStringAsync());
            var result = await GetImageAsync(deepResult.output_url);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }
    }
}

