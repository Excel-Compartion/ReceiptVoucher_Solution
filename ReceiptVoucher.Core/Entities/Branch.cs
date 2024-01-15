namespace ReceiptVoucher.Core.Entities;

public class Branch
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "يرجى ادخال اسم الفرع ")]
    [StringLength(250)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "يرجى ادخال اسم المنطقة ")]
    [StringLength(250)]
    public string Area { get; set; } = null!;

    public int? Mobile { get; set; }

    [StringLength(250)]
    [EmailAddress(ErrorMessage = "يرجى ادخال بريد الكتروني صحيح")]
    public string? Email { get; set; }


    public string? ResponsiblePerson { get; set; }

    public bool IsActive { get; set; } = true;

    
    public DateTime CreatedDate { get; set; }


    //-------- Navigation Properties ---

    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
