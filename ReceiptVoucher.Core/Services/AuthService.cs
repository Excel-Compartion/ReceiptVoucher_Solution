using Microsoft.AspNetCore.Identity;
//using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ReceiptVoucher.Core.Services;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Helper;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Models.ResponseModels;

namespace ReceiptVoucher.Core.Services
{
    public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly JWT _jwt;

		public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_jwt = jwt.Value;
			_roleManager = roleManager;
		}



		public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
		{
			var authModel = new AuthModel();

			var user = await _userManager.FindByEmailAsync(model.Email);

			// check first if user is in db or if password is not correct
			
			if(user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				authModel.Message = "Email Or Password is incorrect!";
				return authModel;
			}

			// generate Token
			var jwtSecurityToken = await CreateJwtToken(user);
			IList<string> roleList = await _userManager.GetRolesAsync(user);

			// Map values into AuthModel
			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			//authModel.ExpiresOn = jwtSecurityToken.ValidTo;
			authModel.Roles = roleList.ToList();



			// check if user has refreshToken Active
			if (user.RefreshTokens.Any( t => t.IsActive) )
			{
				RefreshToken? activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);

				authModel.RefreshToken = activeRefreshToken.Token;
				authModel.RefreshTokenExpiration = activeRefreshToken.ExperesOn;
			}
			// if has no RefreshToken , then Generate new
			else
			{
				var refreshToken = GenerateRefreshToken();

				authModel.RefreshToken = refreshToken.Token;
				authModel.RefreshTokenExpiration = refreshToken.ExperesOn;

				// then save this new refresh Token in user table
				user.RefreshTokens.Add(refreshToken);
				await _userManager.UpdateAsync(user);
			}

			return authModel;
		}

		public async Task<AuthModel> RegisterAsync(RegisterModel registerModel)
		{
			// find user by email
			if (await _userManager.FindByEmailAsync(registerModel.Email) is not null ) // means its has been  used.
				return new AuthModel { Message = "Email Already Registered! " };

			// find user by UserName
			if (await _userManager.FindByNameAsync(registerModel.UserName) is not null) // means its has been  used.
				return new AuthModel { Message = "UserName Already Registered! " };


			var user = new ApplicationUser
			{
				UserName = registerModel.UserName, 
				Email = registerModel.Email,
				FirstName = registerModel.FirstName,
				LastName = registerModel.LastName
			};

			
			var result =await _userManager.CreateAsync(user, registerModel.Password);

			// if there is errors
			if (!result.Succeeded)
			{
				var errors = string.Empty;

				foreach (var error in result.Errors)
				{
					errors += $"{error.Description},";
				}

				return new AuthModel { Message = errors };
			}

			// if succeeded . add user to role of user
			await _userManager.AddToRoleAsync(user, RolesNames.User);

			// Generate Token
			var jwtSecurityToken = await CreateJwtToken(user);

			return new AuthModel
			{
				Email = user.Email,
				//ExpiresOn = jwtSecurityToken.ValidTo,
				IsAuthenticated = true,
				Roles = new List<string> { RolesNames.User },
				UserName = user.UserName,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
			};

		}

		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim (JwtRegisteredClaimNames.Sub , user.UserName),
				new Claim (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
				new Claim (JwtRegisteredClaimNames.Email , user.Email),
				new Claim ("uid" , user.Id)

			}.Union(userClaims).Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signinCredentials = new SigningCredentials(symmetricSecurityKey , SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddDays(_jwt.DurationInDays),
				signingCredentials: signinCredentials);

			return jwtSecurityToken;
		}

		public async Task<string> AddRoleAsync(AddRoleModel model)
		{
			// check if user not in db , // check that role name exsist.
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
				return "Invalid User Id Or Role Is Invalid ";

			// check if user already in that role
			if (await _userManager.IsInRoleAsync(user, model.RoleName))
				return "User Already Assigned to this Role";

			// now ok User In DB And He isn't Assignd in that Role.
			var result = await _userManager.AddToRoleAsync(user, model.RoleName);

			return result.Succeeded ? string.Empty : "Some Thing Went Wrong ";

		}

		private RefreshToken GenerateRefreshToken()
		{
			var rundomNumber = new byte[32];

			using var generator = new RNGCryptoServiceProvider();

			generator.GetBytes(rundomNumber);

			return new RefreshToken
			{
				Token = Convert.ToBase64String(rundomNumber),
				ExperesOn = DateTime.UtcNow.AddMinutes(1),
				CreatedOn = DateTime.UtcNow,
			};
		}

		public async Task<AuthModel> RefreshTokenAsync(string token)
		{
			var authModel = new AuthModel();

			// get user that has 
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
			{
				authModel.IsAuthenticated = false;
				authModel.Message = "Invalid Token";

				return authModel;
			}

			// 
			var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

			if (!refreshToken.IsActive)
			{
				authModel.IsAuthenticated = false;
				authModel.Message = "InActive Token";

				return authModel; 
			}

			// Revoke RefreshToken
			refreshToken.RevokedOn = DateTime.UtcNow;

			var newRefreshToken = GenerateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);
			await _userManager.UpdateAsync(user);

			var jwtToken = await CreateJwtToken(user);
			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			var roles = await _userManager.GetRolesAsync(user);
			authModel.Roles = roles.ToList();
			authModel.RefreshToken = newRefreshToken.Token;
			authModel.RefreshTokenExpiration = newRefreshToken.ExperesOn;



			return authModel;
		}

		public async Task<bool> RevokeTokenAsync(string token)
		{
			// get user that has 
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
				return false;

			// 
			var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

			if (!refreshToken.IsActive)
				return false;

			refreshToken.RevokedOn = DateTime.UtcNow;

			await _userManager.UpdateAsync(user);

			return true;
		}

        public async Task<BaseResponse<bool>> ChangePassword(string userId,string currentPassword ,string newPassword)
        {
            // Find the user by id
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);


            // Check if the user exists
            if (user is null )
			{
				return new BaseResponse<bool>(false, "User Not Found !", null, false);
			}

            // Change the user's password

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);


			// Check if the password change was successful
			if (result.Succeeded)
			{
				return new BaseResponse<bool>(true, "Password changed successfully", null, true);
			}
			else
			{
				return new BaseResponse<bool>(false, result.Errors.FirstOrDefault()?.Description , null , false);
			}

        }

       
    }
}
