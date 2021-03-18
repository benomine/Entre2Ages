using MessageMicroservice.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageMicroservice.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _messages = db.GetCollection<Message>(settings.CollectionName);
        }

        public async Task<List<Message>> GetAllAsync()
        {
            return await _messages.Find(Builders<Message>.Filter.Empty).ToListAsync();
        }

        public async Task<Message> GetByIdAsync(string id)
        {
            return await _messages.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetByAuthorAsync(string author)
        {
            return await _messages.Find(m => m.Author == author).ToListAsync();
        }

        public Task Insert(Message message)
        {
            return _messages.InsertOneAsync(message);
        }

        public async Task<bool> DeleteAll()
        {
            var size = await _messages.EstimatedDocumentCountAsync();
            var result = await _messages.DeleteManyAsync(Builders<Message>.Filter.Empty);
            return await Task.FromResult(result.IsAcknowledged && result.DeletedCount == size);
        }

        public async Task<bool> DeleteById(string id)
        {
            var result = await _messages.DeleteOneAsync(m => m.Id == id);
            return await Task.FromResult(result.IsAcknowledged && result.DeletedCount > 0);
        }
        
        public async Task<Message> UpdateById(string id, Message message)
        {
            return await _messages.FindOneAndReplaceAsync(m => m.Id == id, message);
        }
    }
}
