
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using ReceiptVoucher.Client.Services;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;

namespace ReceiptVoucher.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackbar;
        public AuthService(HttpClient httpClient, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
            _snackbar = snackbar;
        }


        public async Task<BaseResponse<AuthModel>> LoginAsync(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Auth/token", loginModel);
            return await result.Content.ReadFromJsonAsync<BaseResponse<AuthModel>>();
        }

        public async Task<BaseResponse<AuthModel>> RegisterAsync(RegisterModel registerModelmodel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Auth/register", registerModelmodel);
            return await result.Content.ReadFromJsonAsync<BaseResponse<AuthModel>>();
        }
        public async Task<BaseResponse<bool>> ChangePassword(UserChangePassword userChangePasswordModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Auth/change-password", userChangePasswordModel);

            return await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();
        }

        public async Task CheckIfNotAdminRedirectToLoginAsync()
        {
            var authState = await _authenticationStateProvider?.GetAuthenticationStateAsync();
            var user = authState?.User;
            var role = user?.FindFirst("roles")?.Value;

            if (role?.Equals(RolesNames.User) == true)
            {
                _navigationManager?.NavigateTo("Receipt");
                _snackbar?.Add("لاتسمح لك صلاحيتك بالوصول الى هذه الصفحه", Severity.Error);

                return;
            }

            else if (user?.Identity?.IsAuthenticated == false || role?.Equals(RolesNames.Admin) == false)
            {
                _navigationManager?.NavigateTo("login");
                _navigationManager?.Refresh();
                _snackbar?.Add("انت لست مسجل الدخول اصلا لكي تصل الى هذه الصفحه,النظام محمي بقوه", Severity.Error);
            }
        }
    }
}
