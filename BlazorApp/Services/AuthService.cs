using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RegisterRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class UserDto
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        // Mock users - in a real app, this would be stored in a database
        private static List<UserWithPassword> _mockUsers = new List<UserWithPassword>
        {
            new UserWithPassword { Id = "1", Name = "Test User", Email = "user@example.com", Password = "password123" }
        };

        private class UserWithPassword : UserDto
        {
            public string Password { get; set; } = string.Empty;
        }

        public async Task<UserDto?> LoginAsync(string email, string password)
        {
            // In a real app, this would make an HTTP request to an API
            // var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginRequest { Email = email, Password = password });
            // response.EnsureSuccessStatusCode();
            // var user = await response.Content.ReadFromJsonAsync<UserDto>();

            // Mock implementation
            await Task.Delay(800); // Simulate network delay
            var user = _mockUsers.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return null;
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            // Store user in local storage and update auth state
            await _authStateProvider.UpdateAuthenticationStateAsync(userDto);

            return userDto;
        }

        public async Task<UserDto?> RegisterAsync(string name, string email, string password)
        {
            // In a real app, this would make an HTTP request to an API
            // var response = await _httpClient.PostAsJsonAsync("api/auth/register", new RegisterRequest { Name = name, Email = email, Password = password });
            // response.EnsureSuccessStatusCode();
            // var user = await response.Content.ReadFromJsonAsync<UserDto>();

            // Mock implementation
            await Task.Delay(800); // Simulate network delay

            // Check if user already exists
            if (_mockUsers.Any(u => u.Email == email))
            {
                return null;
            }

            // Create new user
            var newUser = new UserWithPassword
            {
                Id = (_mockUsers.Count + 1).ToString(),
                Name = name,
                Email = email,
                Password = password
            };

            _mockUsers.Add(newUser);

            var userDto = new UserDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };

            // Store user in local storage and update auth state
            await _authStateProvider.UpdateAuthenticationStateAsync(userDto);

            return userDto;
        }

        public async Task LogoutAsync()
        {
            // In a real app, this might make an HTTP request to invalidate the token
            // await _httpClient.PostAsync("api/auth/logout", null);

            await _authStateProvider.UpdateAuthenticationStateAsync(null);
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                Name = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty
            };
        }

        public bool IsLoggedIn()
        {
            return _authStateProvider.IsAuthenticated();
        }
    }
}
