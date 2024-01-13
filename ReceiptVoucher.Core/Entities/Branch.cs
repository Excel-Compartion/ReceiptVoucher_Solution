namespace ReceiptVoucher.Core.Entities;

public class Branch
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedDate { get; set; }


    //-------- Navigation Properties ---

    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
