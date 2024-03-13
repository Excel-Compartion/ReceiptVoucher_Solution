using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Models;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;
using System.Security.Claims;


namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesNames.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ReceiptVoucherDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthenticationStateProvider _authenticationStateProvider;



        public UsersController(UserManager<ApplicationUser> userManager, ReceiptVoucherDbContext context, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, AuthenticationStateProvider authenticationStateProvider)
        {
            _userManager = userManager;
            this._context = context;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _authenticationStateProvider = authenticationStateProvider;
        }




        //[HttpGet("GetUserById")]
        //public async Task<IActionResult> GetUserById(string Id)
        //{

        //    var user = await _userManager.Users.Where(x=>x.Id==Id).FirstOrDefaultAsync();




        //    if (user == null)
        //    {
        //        return (BadRequest("user Not Found"));
        //    }

        //    var userViewModels = new UserViewModel()
        //    {
        //        Id = user.Id,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email,
        //        BranchId = user.BranchId,
        //        UserName = user.UserName,


        //    };

        //    return Ok(userViewModels);

        //}


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
                    Roles = userRoles,
                    Mobile= user.PhoneNumber,
                    IsEnabled = user.IsEnabled
                    
                    
                });
            }

            return Ok(userViewModels);

        

        }


        [HttpGet("GetUserDetails/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<UserViewModel>>> GetUserDetails(string userId)
        {



            try
            {
                // Query the database for the user
                var applicationUser = await _unitOfWork.Users.FindAsync(u => u.Id == userId, [nameof(Branch)]);

                if (applicationUser == null)
                {
                    return NotFound();
                }

                var userVM = new UserViewModel
                {
                    Id = applicationUser.Id,
                    UserName = applicationUser.UserName,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Mobile = applicationUser.PhoneNumber,
                    Email = applicationUser.Email,
                    BranchId = applicationUser.BranchId,
                    BranchName = applicationUser.Branch != null ? applicationUser.Branch.Name : "ليس مرتبط بمكتب",
                    BranchAccountNumber = applicationUser.Branch != null ? applicationUser.Branch.AccountNumber : 0,
                    IsEnabled = applicationUser.IsEnabled,
                    Roles = _userManager.GetRolesAsync(applicationUser).Result
                };




                return Ok( new BaseResponse<UserViewModel>(userVM , " تم جلب بيانات المستخدم", null , true ) );
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<UserViewModel>(null, ex.Message , null, false));


            }



            //// Get the current user's ID and roles from the User property of the HttpContext
            //var userId = User.FindFirstValue("uid");

            //var roles = User.FindFirstValue(ClaimTypes.Role);
            //// Split the roles string into a list
            //var roleList = roles?.Split(',').ToList();
            //userVM.Roles = roleList;

            ////// Query the database for the user using the Unit of Work
            ////ApplicationUser userInDB = await _unitOfWork.Users.FindAsync(u => u.Id == userId);

            ////if (userInDB == null)
            ////{
            ////    return NotFound();
            ////}

            ////// Create the UserViewModel
            ////var currentUser = new UserViewModel()
            ////{
            ////    Id = userInDB.Id,
            ////    Email = userInDB.Email,
            ////    BranchId = userInDB.BranchId,
            ////    FirstName = userInDB.FirstName,
            ////    LastName = userInDB.LastName,
            ////    UserName = userInDB.UserName,
            ////    BranchName
            ////    Roles = roleList  // Assign all roles to the UserViewModel
            ////};

            //return Ok(userVM);
        }


        [HttpPost]
        public async Task<ActionResult> CreateUser(  CreateUserModel model)
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
                BranchId = model.BranchId,
                PhoneNumber=model.Mobile,
                IsEnabled =  true
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
        public async Task<IActionResult> UpdateUser( EditProfileViewModel model)
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
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "This email is already in use.", errors: ModelErrors, success: false));
            }

            // Check if username is already in use by another user
            var existingUserByUsername = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserByUsername != null && existingUserByUsername.Id != model.UserId)
            {
                ModelState.AddModelError("UserName", "This username is already in use.");
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "", errors: ModelErrors, success: false));

            }

            // Update user information
            userInDB.FirstName = model.FirstName;
            userInDB.LastName = model.LastName;
            userInDB.UserName = model.UserName;
            userInDB.Email = model.Email;
            userInDB.BranchId = model.BranchId;
            userInDB.PhoneNumber = model.Mobile;
            userInDB.IsEnabled = model.IsEnabled;

            var result = await _userManager.UpdateAsync(userInDB);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<string>(null, "حدث خطاء اثناء عملية التعديل", errors: ModelErrors, success: false));

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
                IsEnabled = userInDB.IsEnabled
                
                
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
                return NotFound(new BaseResponse<string>(null, ".المستخدم الذي تريد حذفه غير موجود", null, false));
            }

            // Check if the user has any Receipt records
            var hasReceipts = await _unitOfWork.Receipts.AnyAsync(r => r.ReceivedBy == user.Id);
            if (hasReceipts)
            {
                return BadRequest(new BaseResponse<string>(null, "لايمكن حذف هذا المستخدم, لأن المستخدم مرتبط بسندات قام بأنشائها", null, false));
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new BaseResponse<string>(null, "حدث خطاء اثناء حذف المستخدم.", errors, false));
            }

            return Ok(new BaseResponse<string>(null, "تم حذف المستخدم بنجاح.", null, true));
        }


        [HttpPost("ChangeUserStatus/{id}")]
        public async Task<IActionResult> ChangeUserStatus(  string id )
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new BaseResponse<string>(null, "هذا الحساب غير موجود", null, false) );
            }

            // Toggle the user's enabled status
            user.IsEnabled = !user.IsEnabled;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var message = user.IsEnabled ? "تم تفعيل حساب المستخدم" : "تم الغاء تفعيل حساب المستخدم";
                return Ok(new BaseResponse<string>(null, message , null, true));
            }

            var IdentityErrors = result.Errors.Select(error => error.Description).ToList();

            return BadRequest(new BaseResponse<string>(null, "حدث خطاء اثناء تغيير حالة الحساب" , IdentityErrors , false));
        }

    }

}
