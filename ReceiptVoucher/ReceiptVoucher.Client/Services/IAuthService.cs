using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;

namespace ReceiptVoucher.Client.Services
{
    public interface IAuthService
	{
		Task<BaseResponse<AuthModel>> RegisterAsync(RegisterModel registerModel);
		Task<BaseResponse<AuthModel>> LoginAsync(LoginModel loginModel);
        Task<BaseResponse<bool>> ChangePassword(UserChangePassword userChangePasswordModel);

        Task CheckIfNotAdminRedirectToLoginAsync();

        Task<UserViewModel?> GetCurrentUserDetailsFromTokenAsync();
        Task<UserViewModel?> GetCurrentUserDetailsFromApiAsync();
        Task<UserViewModel?> GetUserDetailsByIdAsync(string userId);

        /// <summary>
        /// <b>WALEED MOHAMMED</b> Edited At 2024-03-015
        /// <br></br>
        /// firstName And Last Name الخاص بالمستخدم بتمرير  ID هذه الداله تقوم بارجاع 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        Task<string?> GetUserIdByFullNameAsync(string firstName, string lastName);

        //Task<string> AddRoleAsync(AddRoleModel roleModel);
        //Task<AuthModel> RefreshTokenAsync(string token);
        //Task<bool> RevokeTokenAsync(string token);
    }
}
