using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels.UserModels
{
   
    public class EditProfileViewModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]

        public string? FirstName { get; set; }

    

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
     
        public string? LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
       
        public string UserName { get; set; } = default!;

        [Required]
        [EmailAddress]
        
        public string? Email { get; set; }

        public int? BranchId { get; set; }

        public RoleViewModel? Role { get; set; }

    }
}

