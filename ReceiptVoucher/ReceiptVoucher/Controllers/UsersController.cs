using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = RolesNames.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = userRoles
                });
            }

            return Ok(userViewModels);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // if no selected Roles
            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please Select At Least One Role");

                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage ).ToList();

                return BadRequest(new BaseResponse<string>(null, "Errors in Model" , errors: ModelErrors , success: false));
            }

            // check if Email Not Exists Before
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "الأيميل مستخدم من قبل!");
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "خطاء متعلق بوجود الأيميل من قبل", errors: ModelErrors, success: false));
            }

            // check if UserName Not Exists Before
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "اسم المستخدم موجود من قبل!");
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "خطاء متعلق بوجود اسم المستخدم من قبل", errors: ModelErrors, success: false));
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("CreateUserResult", error.Description);
                }

                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "أخطاء متعلقه بأنشاء الحساب للمستخدم ", errors: ModelErrors, success: false));
            }

            // ok user is created but Assign Roles
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            return Ok(new BaseResponse<ApplicationUser>(user , "تم انشاء حساب المستخدم بنجاح", null , true));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(EditProfileViewModel model)
        {
            ///<summary>
            /// 1 - check if user is in DB if not return not found.
            /// 2 - after that we have two cases:
            ///       1. may Admin Editing user and put used Email or User Name And We Want To Prevent this 
            ///       2. Admin Editing User And put newly UserName Or Email This is OK.
            ///</summary>

            ApplicationUser userInDB = await _userManager.FindByIdAsync(model.UserId);

            // check if null
            if (userInDB == null)
                return NotFound();

            // get user by Email of ViewModel
            ApplicationUser userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            // now i want to check if that userWithSameEmail 
            // this line means i have userInDB With Same Email && but Id Of User With Same Email Not like Id of User coming From ViewModel !
            if (userWithSameEmail != null && userWithSameEmail.Id != model.UserId)
            {
                ModelState.AddModelError("Email", "هذا الأيميل مستخدم من قبل مع شخص أخر في النظام!");

                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "خطاء في الأيميل", errors: ModelErrors, success: false));
            }



            // get user by UserName of ViewModel
            ApplicationUser userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);


            if (userWithSameUserName != null && userWithSameUserName.Id != model.UserId)
            {
                ModelState.AddModelError("UserName", "اسم المستخدم هذا موجود من قبل مع شخص أخر في النظام");

                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "خطاء في اسم المستخدم", errors: ModelErrors, success: false));
            }


            // Now Every Thing Is OK, So Update Values, And Take Care Do not Change or Update the ID
            userInDB.FirstName = model.FirstName;
            userInDB.LastName = model.LastName;
            userInDB.UserName = model.UserName;
            userInDB.Email = model.Email;
            await _userManager.UpdateAsync(userInDB);

            // Create a new instance of ApplicationUser with the desired properties
            var updatedUser = new UserResponse
            {
                FirstName = userInDB.FirstName,
                LastName = userInDB.LastName,
                UserName = userInDB.UserName,
                Email = userInDB.Email
            };


            return Ok(new BaseResponse<UserResponse>(updatedUser, "تم تعديل بيانات حساب المستخدم بنجاح", null, true));
        }

        [HttpPost("ManageUserRoles")]
        public async Task<IActionResult> ManageUserRoles(UserRolesViewModel model)
        {

            // Validate the model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);

            // check if null
            if (user == null)
                return NotFound();

            // Get the existing roles assigned to the user
            var userRoles = await _userManager.GetRolesAsync(user);

            // Compare the existing roles with the roles in the request
            foreach (var role in model.Roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role.RoleName);

                // Case 1: If the role already exists in the userRoles and is not selected in the request, remove it from userRoles
                if (roleExists && userRoles.Contains(role.RoleName) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }

                // Case 2: If the role does not exist in userRoles and is selected in the request, add it to userRoles
                if (roleExists && !userRoles.Contains(role.RoleName) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
            }

            return Ok(new BaseResponse<string>(null, "تم تعديل صلاحيات حساب المستخدم بنجاح", null, true));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var userInDB = await _userManager.FindByIdAsync(userId);
            if (userInDB == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(userInDB);
            if (!result.Succeeded)
                throw new Exception();

            // Good
            return Ok(new BaseResponse<string>(null, "تم حذف حساب المستخدم بنجاح", null, true));
        }
    }
    
}
