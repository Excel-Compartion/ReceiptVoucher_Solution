using Microsoft.AspNetCore.Identity;
using ReceiptVoucher.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ReceiptVoucher.Core.Identity
{
	public class ApplicationUser : IdentityUser
	{
        [Required , MaxLength (50)]
        public string FirstName { get; set; }


		[Required, MaxLength(50)]
		public string LastName { get; set; }


        public int? BranchId { get; set; }   // Foreign Key , except null means this will be Admin


        public bool IsEnabled { get; set; } = false ;    // defualt

        //-------- Navigation Properties ---

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
