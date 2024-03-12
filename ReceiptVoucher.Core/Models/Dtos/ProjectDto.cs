using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.Dtos
{
    public class GetProjectDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المشروع")]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        public string? Note { get; set; }

        public bool IsActive { get; set; } = true;

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }

    public class PostProjectDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المشروع")]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        public string? Note { get; set; }

        public bool IsActive { get; set; } = true;

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
