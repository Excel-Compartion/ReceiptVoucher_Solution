
namespace ReceiptVoucher.Core.Models;

public  class Branch
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedDate { get; set; }


    //-------- Navigation Properties ---

    public  ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
