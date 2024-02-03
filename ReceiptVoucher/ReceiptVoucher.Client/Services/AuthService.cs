
using System.Net.Http;
using System.Net.Http.Json;
using ReceiptVoucher.Client.Services;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;

namespace ReceiptVoucher.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService( HttpClient httpClient )
        {
            _httpClient = httpClient;
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

    }
}
