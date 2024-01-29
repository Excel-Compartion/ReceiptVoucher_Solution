using System.ComponentModel.DataAnnotations;

namespace ReceiptVoucher.Core.Models.Dtos.Auth
{
	public class TokenRequestModel
	{
        [Required(ErrorMessage = "يرجى اخال البريد الالكتروني ")]
        //[EmailAddress(ErrorMessage = "يرجى ادخال البريد الالكتروني بشكل صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يرجى اخال كلمه السر")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
