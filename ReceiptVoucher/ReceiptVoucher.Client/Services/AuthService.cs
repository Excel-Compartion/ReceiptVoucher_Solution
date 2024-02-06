
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
        public AuthService(HttpClient httpClient, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
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
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var role = user.FindFirst("roles")?.Value;

            if (role.Equals(RolesNames.User))
            {
                _navigationManager.NavigateTo("Receipt");
                return;
            }

            else if (!user.Identity.IsAuthenticated || !role.Equals(RolesNames.Admin))
            {
                _navigationManager.NavigateTo("login");
                _navigationManager.Refresh();

            }
        }
    }
}
