namespace ReceiptVoucher.Core.Models;

[Table("CompanyInfo")]
public partial class CompanyInfo
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; } = null!;

    public int? Mobile { get; set; }

    public int? LicenseNumber { get; set; }

    public int? Telephone { get; set; }

    [StringLength(250)]
    public string? Email { get; set; }

    [StringLength(250)]
    public string? Web { get; set; }

    [StringLength(250)]
    public string? IVT { get; set; }
}
