using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Entities
{
    public class DTO
    {
        public int SubProjectId { get; set; }

        public int ProjectId { get; set; }  // Foreign Key

        [Required(ErrorMessage = "حقل الاسم مطلوب")]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        public int? Duration { get; set; }

    }
}
