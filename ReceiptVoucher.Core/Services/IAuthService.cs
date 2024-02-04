using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;

namespace ReceiptVoucher.Core.Services
{
    public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(RegisterModel registerModel);
		Task<AuthModel> GetTokenAsync(TokenRequestModel tokenRequestModel);
		Task<string> AddRoleAsync(AddRoleModel roleModel);
		Task<AuthModel> RefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
		Task<BaseResponse<bool>> ChangePassword(string userId ,string currentPassword, string newPassword);
	}
}
