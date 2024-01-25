namespace ReceiptVoucher.Core.Entities;

[Table("CompanyInfo")]
public class CompanyInfo
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage ="يرجى ادخال اسم الشركه")]
    [StringLength(250)]
    public string Name { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? LicenseNumber { get; set; }

    public string? Telephone { get; set; }

    [StringLength(250)]
    [EmailAddress(ErrorMessage = "يرجى ادخال بريد الكتروني صحيح")]
    public string? Email { get; set; }

    [StringLength(250)]
    public string? Web { get; set; }

    [StringLength(250)]
    public string? IVT { get; set; }
}
