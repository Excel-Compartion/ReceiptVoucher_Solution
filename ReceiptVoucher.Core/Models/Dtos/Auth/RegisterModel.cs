using System.ComponentModel.DataAnnotations;

namespace ReceiptVoucher.Core.Models.Dtos.Auth
{
	public class RegisterModel
	{
        [Required, StringLength(100)]
        public string FirstName { get; set; }


        [Required, StringLength(100)]
        public string lastName { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }


        [Required, StringLength(100)]
        public string Email { get; set; }


        [Required, StringLength(100)]   
        public string Password { get; set; }
    }
}
