
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using ReceiptVoucher.Client.Services;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;
using static System.Net.WebRequestMethods;

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
        private readonly ILocalStorageService _localStorageService;

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

        public async Task<UserViewModel?> GetCurrentUserDetailsFromTokenAsync()
        {
            // New Implementation
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState?.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                // Extract claims directly from the authentication state
                var userName = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                var email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
                var userId = user.FindFirst("uid")?.Value; // Assuming "uid" claim is present
                var firstName = user.FindFirstValue(JwtRegisteredClaimNames.GivenName);
                var lastName = user.FindFirstValue(JwtRegisteredClaimNames.FamilyName);
                var phoneNumber = user.FindFirst("PhoneNumber")?.Value;
                var branchIdClaim = user.FindFirst("branchId")?.Value; // Assuming "branchId" claim is present
                var isEnabled = bool.Parse(user.FindFirst("isEnabled")?.Value ?? "false"); // Assuming "isEnabled" claim is present
                var roleClaim = user.FindFirst("roles");


                // Parse branchId to int
                int? branchId = null;
                if (branchIdClaim != null)
                {
                    if (int.TryParse(branchIdClaim, out var parsedBranchId))
                    {
                        branchId = parsedBranchId;
                    }
                    else
                    {
                        // Handle parsing failure, e.g., assign a default value or log an error
                    }
                }



                var currentUser = new UserViewModel
                {
                    UserName = userName ?? "Unknown",
                    Email = email ?? "Unknown",
                    Id = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    BranchId = branchId,
                    IsEnabled = isEnabled,
                    Roles = new List<string>() { roleClaim.Value },
                    //Mobile = phoneNumber
                    
                };

                return currentUser;
            }
            else
            {
                // Handle non-authenticated scenario
                //_logger.LogInformation("User is not authenticated.");
                return null; // Or provide a default UserViewModel if needed
            }


        }

        public async Task<UserViewModel?> GetCurrentUserDetailsFromApiAsync()
        {
            try
            {
                string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                var UserResponse = await _httpClient.GetFromJsonAsync<UserViewModel>("api/users/GetUserDetails");
                return UserResponse;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null; // Or provide a default value
            }
        }
    }
}
