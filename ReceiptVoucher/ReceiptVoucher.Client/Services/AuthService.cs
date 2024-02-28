
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using ReceiptVoucher.Client.Services;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;

namespace ReceiptVoucher.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackbar;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthService(HttpClient httpClient, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider, ISnackbar snackbar, UserManager<ApplicationUser> userManager)
        {
            _httpClient = httpClient;
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
            var authState = await _authenticationStateProvider?.GetAuthenticationStateAsync();
            var user = authState?.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirstValue("uid");

                var UserInDB = await _userManager.Users.Include(user => user.Branch).SingleOrDefaultAsync(u => u.Id == userId);
                if (UserInDB != null)
                {
                    var roles = await _userManager.GetRolesAsync(UserInDB); // Get all roles of the user


                  
                    var currentUser = new UserViewModel()
                    {
                        Id = UserInDB.Id,
                        BranchId = UserInDB.BranchId,
                        Email = UserInDB.Email,
                        FirstName = UserInDB.FirstName,
                        LastName = UserInDB.LastName,
                        UserName = UserInDB.UserName,
                        Roles = roles.ToList() // Assign all roles to the UserViewModel
                    };

                    return currentUser;
                  
                }
            }

            return null; // Return null if user is not authenticated or user is not found in DB
        }

    }
}
