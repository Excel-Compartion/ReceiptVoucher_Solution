using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models;
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
                    BranchId = user.BranchId,
                    UserName = user.UserName,
                    Roles = userRoles
                });
            }

            return Ok(userViewModels);

            return Ok();

        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "Errors in Model", errors: ModelErrors, success: false));
            }

            // Check if role exists in the database
            if (! await _roleManager.RoleExistsAsync(model.Role.RoleName))
            {
                ModelState.AddModelError("Role", "The role does not exist.");
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "خطاء متعلق بعدم وجود الصلاحيه من قبل", errors: ModelErrors, success: false));
            }


            // check if Email Not Exists Before , if = null means there no user have that email. and this ok.
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

            var userToDB = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BranchId = model.BranchId
            };

            var result = await _userManager.CreateAsync(userToDB, model.Password);

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
            await _userManager.AddToRoleAsync(userToDB, model.Role.RoleName);

            // Create a new instance of ApplicationUser with the desired properties
            var updatedUser = new UserResponse
            {
                FirstName = userToDB.FirstName,
                LastName = userToDB.LastName,
                UserName = userToDB.UserName,
                Email = userToDB.Email,
                RoleName = model.Role.RoleName,
                BranchId = userToDB.BranchId
            };


            return Ok(new BaseResponse<UserResponse>(updatedUser, "تم انشاء حساب المستخدم بنجاح", null, true));

        }



        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] EditProfileViewModel model)
        {
            ///<summary>
            /// 1 - check if user is in DB if not return not found.
            /// 2 - after that we have two cases:
            ///       1. may Admin Editing user and put used Email or User Name And We Want To Prevent this 
            ///       2. Admin Editing User And put newly UserName Or Email This is OK.
            ///</summary>
            

            if (!ModelState.IsValid)
            {
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "Errors in Model", errors: ModelErrors, success: false));

            }

            // Check if role exists in the database
            if (!await _roleManager.RoleExistsAsync(model.Role.RoleName))
            {
                ModelState.AddModelError("Role", "The role does not exist.");
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "خطاء متعلق بعدم وجود الصلاحيه في قاعدة البيانات", errors: ModelErrors, success: false));
            }

            // Check if user exists in the database
            var userInDB = await _userManager.FindByIdAsync(model.UserId);
            if (userInDB == null)
                return NotFound(new BaseResponse<string>(null, "هذا المستخدم غير موجود في قاعدة البيانات", null, success: false));


            // Check if email is already in use by another user
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != model.UserId)
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return BadRequest(ModelState);
            }

            // Check if username is already in use by another user
            var existingUserByUsername = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserByUsername != null && existingUserByUsername.Id != model.UserId)
            {
                ModelState.AddModelError("UserName", "This username is already in use.");
                return BadRequest(ModelState);
            }

            // Update user information
            userInDB.FirstName = model.FirstName;
            userInDB.LastName = model.LastName;
            userInDB.UserName = model.UserName;
            userInDB.Email = model.Email;
            userInDB.BranchId = model.BranchId;

            var result = await _userManager.UpdateAsync(userInDB);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            // Get current roles of the user
            IList<string> currentRoles = await _userManager.GetRolesAsync(userInDB);


            // If the user's role has changed, update it
            if (!currentRoles.Contains(model.Role.RoleName))
            {
                // Remove all current roles
                await _userManager.RemoveFromRolesAsync(userInDB, currentRoles);

                // Assign the new role
                await _userManager.AddToRoleAsync(userInDB, model.Role.RoleName);
            }

            // Create a new instance of ApplicationUser with the desired properties
            var userResponse = new UserResponse
            {
                FirstName = userInDB.FirstName,
                LastName = userInDB.LastName,
                UserName = userInDB.UserName,
                Email = userInDB.Email,
                RoleName = currentRoles.FirstOrDefault(),
                BranchId = userInDB.BranchId,
            };

            return Ok(new BaseResponse<UserResponse>(userResponse, "User updated successfully.", null, true));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            // Find the user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new BaseResponse<string>(null , "User not found." ,null ,false));
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select( e => e.Description).ToList();
                return BadRequest(new BaseResponse<string>(null, "Error deleting user.", errors, false));
            }

            return Ok(new BaseResponse<string>(null, "User deleted successfully.", null, true));
        }

    }

}
