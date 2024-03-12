
namespace ReceiptVoucher.Core.Models.Dtos
{
    public class GetBranchDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المكتب ")]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "يرجى ادخال اسم المنطقة ")]
        [StringLength(250)]
        public string Area { get; set; } = null!;

        public string? Mobile { get; set; }

        [StringLength(250)]
        [EmailAddress(ErrorMessage = "يرجى ادخال بريد الكتروني صحيح")]
        public string? Email { get; set; }


        public string? ResponsiblePerson { get; set; }

        public bool IsActive { get; set; } = true;


        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "يرجى ادخال رقم نقطة البيع ")]
        public int AccountNumber { get; set; }

        [Required(ErrorMessage = "يرجى ادخال رقم المكتب ")]
        public int BranchNumber { get; set; }

    }

    public class PostBranchDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المكتب ")]
        [StringLength(250)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "يرجى ادخال اسم المنطقة ")]
        [StringLength(250)]
        public string Area { get; set; } = null!;

        public string? Mobile { get; set; }

        [StringLength(250)]
        [EmailAddress(ErrorMessage = "يرجى ادخال بريد الكتروني صحيح")]
        public string? Email { get; set; }


        public string? ResponsiblePerson { get; set; }

        public bool IsActive { get; set; } = true;


        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "يرجى ادخال رقم نقطة البيع ")]
        public int AccountNumber { get; set; }

        [Required(ErrorMessage = "يرجى ادخال رقم المكتب ")]
        public int BranchNumber { get; set; }

    }
}
