using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorEntre2Ages.Models;
using BlazorEntre2Ages.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorEntre2Ages.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IUserService _userService;        
        private readonly HttpClient _httpClient;        

        public CustomAuthenticationStateProvider(
            ILocalStorageService localStorageService, 
            IUserService userService,
            HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _userService = userService;
            _httpClient = httpClient;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {   
            var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");           
            ClaimsIdentity identity;
            if (!string.IsNullOrEmpty(accessToken))
            {
                var user = await _userService.GetUserByAccessTokenAsync(accessToken);
                identity = GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }
            var claimsPrincipal = new ClaimsPrincipal(identity);            

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(User user)
        {
            await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
            await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync("refreshToken");
            await _localStorageService.RemoveItemAsync("accessToken");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private static ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claimsIdentity = new ClaimsIdentity();
            if (user.Email != null)
            { 
                claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),                                   
                }, "apiauth_type");
            }
            return claimsIdentity;
        }
    }
}
