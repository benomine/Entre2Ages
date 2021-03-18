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
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        public MessageService(HttpClient httpClient, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Entre2Ages");
            _httpClient = httpClient;
        }

        public Task<List<Message>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetById(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetByAuthor(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Send(Message message)
        {
            var json = JsonConvert.SerializeObject(message);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.MessageUrl +"api/Messages"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var status = response.StatusCode == HttpStatusCode.Created;
            return status;
        }

        public Task<bool> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}