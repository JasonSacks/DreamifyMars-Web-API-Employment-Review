using DreamInMars.Configuration;
using DreamInMars.Dto;
using DreamInMars.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DreamInMars.Client
{
    public class NasaMarsRoverClient : INasaMarsRoverClient
    {
        private readonly HttpClient _client;
        private readonly NasaConfiguration _configuration;
        
        public NasaMarsRoverClient(HttpClient client, IOptions<NasaConfiguration> configuration)
        {
            _configuration = configuration.Value;
            client.BaseAddress = new Uri(_configuration.ApiPath);
            _client = client;
        }
        
        public async Task<IEnumerable<PhotoPath>> GetRoverImagesAsync(RoverEnums rover, int page, DateTime? earthDate = null)
        {
            var apiKey = _configuration.ApiKey;
            var queryString =  $"";
            var photos = await CallNasaApiAsync(
                $"{Enum.GetName(rover)}/photos?earth_date={earthDate.Value.ToString("yyyy-MM-dd")}&page={page}&api_key={apiKey}");
            return photos.Photos.Select(p => new PhotoPath() { Path = p.ImageSource, Id = p.Id });
        }

        private async Task<MarsPhotos> CallNasaApiAsync(string path)
        {
            var result = await _client.GetAsync(path);
            result.EnsureSuccessStatusCode();
            using var json = await result.Content.ReadAsStreamAsync();
            var photos = await JsonSerializer.DeserializeAsync<MarsPhotos>(json);
            return photos;
        }
    }
}
