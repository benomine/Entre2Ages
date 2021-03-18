using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlazorEntre2Ages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BlazorEntre2Ages.Services
{
    public class UserService : IUserService
    {

        #region Champs
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;
        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="optionsMonitor"></param>
        public UserService(HttpClient httpClient, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Entre2Ages");

            _httpClient = httpClient;
        }


        /// <summary>
        /// Login Method to request a existed User Object
        /// </summary>
        /// <param name="user">Async</param>
        /// <returns>User</returns>
        public async Task<User> LoginAsync(User user)
        {
            user.Password = Encrypt(user.Password);
            var serializedUser = JsonConvert.SerializeObject(user);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.UserUrl+"api/Users/Login"),
                Content = new StringContent(serializedUser, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);
            
            return await Task.FromResult(returnedUser);
        }


        /// <summary>
        /// Register Method: to create a new user, from a confirmation link received as a email.
        /// </summary>
        /// <param name="user">Async</param>
        /// <returns>User</returns>
        public async Task<User> RegisterUserAsync(User user)
        {
            user.Password = Encrypt(user.Password);
            var serializedUser = JsonConvert.SerializeObject(user);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.UserUrl +"api/Users/Register"),
                Content = new StringContent(serializedUser, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(requestMessage);

            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

            return await Task.FromResult(returnedUser);
        }



        /// <summary>
        /// Refresh method
        /// </summary>
        /// <param name="refreshRequest"></param>
        /// <returns></returns>
        public async Task<User> RefreshTokenAsync(RefreshRequest refreshRequest)
        {
            var serializedUser = JsonConvert.SerializeObject(refreshRequest);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.UserUrl+"api/Users/RefreshToken"),
                Content = new StringContent(serializedUser, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(requestMessage);

            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

            return await Task.FromResult(returnedUser);
        }


        /// <summary>
        /// Call the User services to collect the token access of the user. Which is already connected.
        /// </summary>
        /// <param name="accessToken">Async</param>
        /// <returns>User</returns>
        public async Task<User> GetUserByAccessTokenAsync(string accessToken)
        {
            var serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.UserUrl+"api/Users/GetUserByAccessToken"),
                Content = new StringContent(serializedRefreshRequest, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

            return await Task.FromResult(returnedUser);
        }
        

        /// <summary>
        /// Method to encrypte the password 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            var provider = MD5.Create();
            const string salt = "RandomSalt";            
            var bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + password));
            return BitConverter.ToString(bytes).Replace("-","").ToLower();
        }


        /// <summary>
        /// Request from the UserService to return all the User. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAll()
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.UserUrl +"api/Users/"),
            };

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            var returnedUsers = JsonConvert.DeserializeObject<List<User>>(responseBody);
            return returnedUsers;
        }
    }
}