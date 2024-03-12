
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using ReceiptVoucher.Client.Services;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;

namespace ReceiptVoucher.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ReceiptVoucherDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackbar;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthService(HttpClient httpClient, ReceiptVoucherDbContext context , NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider, ISnackbar snackbar, UserManager<ApplicationUser> userManager)
        {
            _httpClient = httpClient;
            _context = context;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
            _snackbar = snackbar;
            _userManager = userManager;
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
                _snackbar?.Add("انت لست مسجل الدخول لكي تصل الى هذه الصفحه", Severity.Error);
            }
        }

        public async Task<UserViewModel?> GetCurrentUserDetailsAsync()
        {
            // New Implementation

            UserViewModel? result = await _httpClient.GetFromJsonAsync<UserViewModel>("api/users/GetUserDetails");

            return result;


        }

    }
}
