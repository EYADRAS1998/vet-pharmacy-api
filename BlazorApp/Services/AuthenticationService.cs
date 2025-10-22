using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorUI.Services
{
    public class AuthenticationService
    {
        private readonly IJSRuntime _js;
        private string _cachedToken;

        public event Func<Task> OnAuthStateChanged;


        public AuthenticationService(IJSRuntime js) => _js = js;

        public async Task SetToken(string token)
        {
            _cachedToken = token;
            await _js.InvokeVoidAsync("localStorage.setItem", "jwtToken", token);
            if (OnAuthStateChanged != null) await OnAuthStateChanged.Invoke();
        }

        public async Task<string> GetTokenAsync(bool forceRefresh = false)
        {
            if (!string.IsNullOrEmpty(_cachedToken) && !forceRefresh)
                return _cachedToken;

            // تحقق مما إذا كنا في prerendering
            if (!_js.IsJSRuntimeAvailable())
                return _cachedToken;

            _cachedToken = await _js.InvokeAsync<string>("localStorage.getItem", "jwtToken");
            return _cachedToken;
        }


        public async Task RemoveToken()
        {
            _cachedToken = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
            if (OnAuthStateChanged != null) await OnAuthStateChanged.Invoke();
        }

        public async Task<bool> IsTokenValidAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token)) return false;

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try { jwtToken = handler.ReadJwtToken(token); }
            catch { return false; }

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim == null) return false;

            var expUnix = long.Parse(expClaim.Value);
            var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
            return expDate > DateTime.UtcNow;
        }

        public async Task<bool> IsUserLoggedInAsync() => await IsTokenValidAsync();

        public async Task<int> GetUserIdAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token)) return 0;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

    }
}
public static class JSRuntimeExtensions
{
    public static bool IsJSRuntimeAvailable(this IJSRuntime js)
    {
        return js != null && js.GetType().Name != "RemoteJSRuntime";
    }
}
