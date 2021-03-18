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
    public class EventService : IEventService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        public EventService(HttpClient httpClient, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Entre2Ages");

            _httpClient = httpClient;
        }

        public async Task<List<Event>> GetAll()
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.EventUrl +"api/Events/"),
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<List<Event>>(responseBody);
            return events;
        }

        public async Task<bool> Create(Event @event)
        {
            var json = JsonConvert.SerializeObject(@event);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.EventUrl +"api/Events"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var status = response.StatusCode == HttpStatusCode.Created;
            return status;
        }

        public async Task<bool> Update(Event @event)
        {
            var json = JsonConvert.SerializeObject(@event);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(_settings.EventUrl +"api/Events/"+@event.Id),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var status = response.StatusCode == HttpStatusCode.NoContent;
            return status;
        }

        public async Task<bool> Delete(Event @event)
        {
            var json = JsonConvert.SerializeObject(@event);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_settings.EventUrl +"api/Events/"+@event.Id),
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var status = response.StatusCode == HttpStatusCode.NoContent;
            return status;
        }
    }
}