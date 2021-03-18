using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageMicroservice.Domain.Models;

namespace MessageMicroservice.Services.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetAllAsync();
        Task<Message> GetByIdAsync(string id);
        Task<List<Message>> GetByAuthorAsync(string author);
        Task Insert(Message message);
        Task<bool> DeleteAll();
        Task<bool> DeleteById(string id);
        Task<Message> UpdateById(string id, Message message);
    }
}