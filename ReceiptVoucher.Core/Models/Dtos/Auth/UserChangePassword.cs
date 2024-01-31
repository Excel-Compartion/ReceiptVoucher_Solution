using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.Dtos.Auth
{
    public class UserChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;


        [Required, StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;


        [Compare(nameof(NewPassword) , ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
