using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Entities
{
    public class LoginModel
    {
        [Required(ErrorMessage = "يرجى اخال البريد الالكتروني ")]
        [EmailAddress(ErrorMessage = "يرجى ادخال البريد الالكتروني بشكل صحيح")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="يرجى اخال كلمه السر")]
        public string Password { get; set; }=null!;

        public bool RemmberMe { get; set; }
    }
}
