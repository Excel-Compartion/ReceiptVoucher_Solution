using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.Dtos.Auth;

namespace ReceiptVoucher.Core.Services
{
	public interface IAuthService
	{
		Task<AuthModel> RegisterAsync(RegisterModel registerModel);
		Task<AuthModel> GetTokenAsync(TokenRequestModel tokenRequestModel);
		Task<string> AddRoleAsync(AddRoleModel roleModel);
		Task<AuthModel> RefreshTokenAsync(string token);
		Task<bool> RevokeTokenAsync(string token);
	}
}
