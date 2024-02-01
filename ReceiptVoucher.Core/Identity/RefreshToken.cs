using Microsoft.EntityFrameworkCore;

namespace ReceiptVoucher.Core.Identity
{
    [Owned]
	public class RefreshToken
	{
        public string Token { get; set; }
        public DateTime ExperesOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExperesOn ;

        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }

        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
