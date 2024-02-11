using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.ResponseModels;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesNames.Admin)]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();

            return Ok(new BaseResponse<List<IdentityRole>>(roles , "تم جلب اسماء جميع الصلاحيات بنجاح", null , true));
        }


        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var roles = await _roleManager.Roles.ToListAsync();

            // if model is not vaild
            if (roleName is null )
            {
                return BadRequest(new BaseResponse<string>(null, "خطاء , يجب ادخال اسم الصلاحيه", null, success: false));
            }



            // if Role Exsist
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest(new BaseResponse<string>(null, "خطاء , الصلاحيه موجوده من قبل ", null, success: false));
            }

            // ok
            await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));

            return Ok(new BaseResponse<string>(null, "تم اضافة الصلاحيه بنجاح", null, true));

        }


        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            // Check if the role exists
            if (role == null)
            {
                return NotFound(new BaseResponse<string>(null, "خطأ، الصلاحية غير موجودة", null, false));
            }

            // Retrieve the users assigned to the role
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            // Check if any users are assigned to the role
            if (usersInRole.Count > 0)
            {
                return BadRequest(new BaseResponse<string>(null, "خطأ، لا يمكن حذف الصلاحية لأنها مرتبطة بمستخدمين", null, false));
            }

            // Delete the role
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(new BaseResponse<string>(null, "تم حذف الصلاحية بنجاح", null, true));
            }
            else
            {
                return BadRequest(new BaseResponse<string>(null, "حدث خطأ أثناء حذف الصلاحية", result.Errors.Select(e => e.Description).ToList(), false));
            }
        }
    }
}
