using ReceiptVoucher.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class GrantDestination_VM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المودع")]
        [StringLength(250)]
        public string ReceivedFrom { get; set; } = null!;

        public GrantDest GrantDestinations { get; set; }

        public string GrantDestinationName => GrantDestinations.GetDisplayName();

        public string? Mobile { get; set; }

        public string Code { get; set; } = null!;

        public int Number { get; set; }

    }
}
