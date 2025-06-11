using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly static string _storageKey = "currentUser";
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private bool? _isAuthenticated;

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _storageKey);

                if (string.IsNullOrEmpty(userJson))
                {
                    _isAuthenticated = false;
                    return new AuthenticationState(_anonymous);
                }

                var user = JsonSerializer.Deserialize<AuthService.UserDto>(userJson);

                if (user == null)
                {
                    _isAuthenticated = false;
                    return new AuthenticationState(_anonymous);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, "BlazorAuth");
                var principal = new ClaimsPrincipal(identity);

                _isAuthenticated = true;
                return new AuthenticationState(principal);
            }
            catch
            {
                _isAuthenticated = false;
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task UpdateAuthenticationStateAsync(AuthService.UserDto? user)
        {
            ClaimsPrincipal principal;

            if (user != null)
            {
                // Store user in local storage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _storageKey, JsonSerializer.Serialize(user));

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, "BlazorAuth");
                principal = new ClaimsPrincipal(identity);
                _isAuthenticated = true;
            }
            else
            {
                // Remove user from local storage
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _storageKey);
                principal = _anonymous;
                _isAuthenticated = false;
            }

            // Notify authentication state changed
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public bool IsAuthenticated()
        {
            return _isAuthenticated ?? false;
        }
    }
}
