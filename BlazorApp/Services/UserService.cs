using Application.DTOs;
using Application.DTOs.UserDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorUI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;

        public UserService(HttpClient httpClient, AuthenticationService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return new ApiResponse { Success = false, Message = errorMsg };
            }

            var msg = await response.Content.ReadAsStringAsync();
            return new ApiResponse { Success = true, Message = msg };
        }
        public async Task<ApiResponse> LoginAsync(LoginUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/login", dto);

            var msg = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new ApiResponse { Success = false, Message = msg };

            await _authService.SetToken(msg);
            return new ApiResponse { Success = true, Message = "تم تسجيل الدخول بنجاح" };
        }

        public async Task LogoutAsync() => await _authService.RemoveToken();

        public async Task<T> GetProtectedDataAsync<T>(string url)
        {
            if (!await _authService.IsTokenValidAsync()) return default;

            var token = await _authService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<T>(url);
        }
    }
}
