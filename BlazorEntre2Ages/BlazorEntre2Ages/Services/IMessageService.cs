using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorEntre2Ages.Models;

namespace BlazorEntre2Ages.Services
{
    public interface IMessageService
    {
        public Task<List<Message>> GetAll();
        public Task<Message> GetById(Guid guid);
        public Task<Message> GetByAuthor(string email);
        public Task<bool> Send(Message message);
        public Task<bool> Delete(Guid guid);
    }
}