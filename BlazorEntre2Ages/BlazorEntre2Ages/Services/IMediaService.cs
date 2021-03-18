using BlazorEntre2Ages.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorEntre2Ages.Services
{
    public interface IMediaService
    {
        Task<List<Media>> GetAll();
        Task<Media> GetById(Guid id);
        Task<bool> Create(Media media);
    }
}
