using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorEntre2Ages.Models;

namespace BlazorEntre2Ages.Services
{
    public interface IEventService
    {
        public Task<List<Event>> GetAll();
        public Task<bool> Create(Event @event);
        public Task<bool> Update(Event @event);
        public Task<bool> Delete(Event @event);
    }
}