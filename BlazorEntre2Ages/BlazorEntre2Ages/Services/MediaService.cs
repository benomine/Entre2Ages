using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorEntre2Ages.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BlazorEntre2Ages.Services
{
    public class MediaService : IMediaService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        public MediaService(HttpClient httpClient, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Entre2Ages");

            _httpClient = httpClient;
        }

        public async Task<List<Media>> GetAll()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.MediaUrl + "/api/Medias")
            };

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var medias = JsonConvert.DeserializeObject<List<Media>>(responseBody);
            return medias;
        }

        public async Task<Media> GetById(Guid id)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.MediaUrl + "/api/Medias/"+id)
            };

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var media = JsonConvert.DeserializeObject<Media>(responseBody);
            return media;
        }
        
        public async Task<bool> Create(Media media)
        {
            var json = JsonConvert.SerializeObject(media);
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.MediaUrl + "/api/Medias"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            var status = response.StatusCode;
            var result = status == HttpStatusCode.Created;
            return result;
        }
    }
}
