using BlazorEntre2Ages.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorEntre2Ages.Services
{
    public interface IChatService
    {
        public List<Message> Messages { get; set; }
        event Func<List<Message>, Task> OnChangeAsync;
        void HandleMessage(Message message);
        Task<List<Message>> GetMessagesAsync();
    }
}