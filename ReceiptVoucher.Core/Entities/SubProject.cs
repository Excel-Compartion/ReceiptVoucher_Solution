using ReceiptVoucher.Core.Enums;

namespace ReceiptVoucher.Core.Entities;

[Table("Sub_Projects")]
public class SubProject
{
    [Key]
    public int Id { get; set; }

    [Range(1, int.MaxValue,ErrorMessage ="--- يرجى اختيار المشروع الرئيسي ---")]
    public int ProjectId { get; set; }  // Foreign Key

    [Required(ErrorMessage ="حقل الاسم مطلوب")]
    [StringLength(250)]
    public string Name { get; set; } = null!;

    public int? Duration { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار نوع المشروع ---")]
    public subProjectType SubProjectType { get; set; }

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string? Note { get; set; }

    public bool IsActive { get; set; } = true;

    //-------- Navigation Properties ---

    [ForeignKey("ProjectId")]

    public Project? Project { get; set; } = null!;

    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
