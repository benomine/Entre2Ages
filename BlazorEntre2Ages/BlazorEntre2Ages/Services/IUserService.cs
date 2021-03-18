using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorEntre2Ages.Models;

namespace BlazorEntre2Ages.Services
{
    public interface IUserService
    {
        public Task<User> LoginAsync(User user);
        public Task<User> RegisterUserAsync(User user);
        public Task<User> GetUserByAccessTokenAsync(string accessToken);
        public Task<User> RefreshTokenAsync(RefreshRequest refreshRequest);
        public Task<List<User>> GetAll();
    }
}