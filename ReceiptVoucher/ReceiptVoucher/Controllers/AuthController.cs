using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReceiptVoucher.Core.Models.Dtos.Auth;
using ReceiptVoucher.Core.Services;
using ReceiptVoucher.Core.Models;
using ReceiptVoucher.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ReceiptVoucher.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync( RegisterModel model)
		{
            //1. check Model Annotations
            if (!ModelState.IsValid)
			{
                List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest( error: new BaseResponse<string>(null , message: "BadRequest Check The Inputs", errors: errors , success: false ) );
			}

            AuthModel result = await _authService.RegisterAsync(model);

            //2. check error form server
            if(!result.IsAuthenticated)
                return BadRequest(new BaseResponse<string>(null , result.Message , success: false));

            return Ok(new BaseResponse<AuthModel>(result, "تم انشاء الحساب بنجاح", success: true));
        }



        [HttpPost("token")]
		public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
		{
            //1. check Model Annotations
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<AuthModel>(null, "Invalid Model, check Some Fields", errors, false));
            }

            AuthModel result = await _authService.GetTokenAsync(model);

            //2. check error form server
            if (!result.IsAuthenticated)
                return BadRequest(new BaseResponse<AuthModel>(null, result.Message, null, false));

            return Ok(new BaseResponse<AuthModel>(result, "Authentication successful", null, true));
        }



        [HttpPost("AddRole")]
		public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
		{
			//1. check Model Annotations
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.AddRoleAsync(model);

			//2. check error form server
			if (!result.IsNullOrEmpty())
				return BadRequest(result);

			return Ok(model); // return data.
		}



        [HttpPost("change-password"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BaseResponse<bool>>> ChangePassword([FromBody] UserChangePassword model )
        {
            var userId = User.FindFirstValue("uid");

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            BaseResponse<bool> response = await _authService.ChangePassword(userId,model.CurrentPassword ,model.NewPassword);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


    }
}
