using ReceiptVoucher.Core.Models.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class CreateUserModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "يجب أن يكون {0} على الأقل {2} و كحد أقصى {1} حرفًا.", MinimumLength = 3)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "يجب أن يكون {0} على الأقل {2} و كحد أقصى {1} حرفًا.", MinimumLength = 3)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string UserName { get; set; } = default!;

        [Required]
        [EmailAddress(ErrorMessage = "صيغة كتابة الأيميل غير صحيحه. مثال : example@gmail.com")]
        public string? Email { get; set; }

        public int? BranchId { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "يجب أن يكون {0} على الأقل {2} و كحد أقصى {1} حرفًا.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور وكلمة التأكيد غير متطابقتان")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Please select a role.")]
        public RoleViewModel? Role { get; set; }


    }
}
