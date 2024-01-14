namespace ReceiptVoucher.Core.Entities;

public class Branch
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "يرجى ادخال اسم الفرع ")]
    [StringLength(250)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    
    public DateTime CreatedDate { get; set; }


    //-------- Navigation Properties ---

    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
