

using ReceiptVoucher.Core.Models;
using ReceiptVoucher.Core.Models.Dtos.Auth;

namespace ReceiptVoucher.Client.Services
{
	public interface IAuthService
	{
		Task<BaseResponse<AuthModel>> RegisterAsync(RegisterModel registerModel);
		Task<BaseResponse<AuthModel>> LoginAsync(LoginModel loginModel);

		//Task<string> AddRoleAsync(AddRoleModel roleModel);
		//Task<AuthModel> RefreshTokenAsync(string token);
		//Task<bool> RevokeTokenAsync(string token);
	}
}
