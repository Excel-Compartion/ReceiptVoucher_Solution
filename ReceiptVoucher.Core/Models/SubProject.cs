namespace ReceiptVoucher.Core.Models;

[Table("Sub_Projects")]
public  class SubProject
{
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }  // Foreign Key

    [StringLength(250)]
    public string Name { get; set; } = null!;

    public int? Duration { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    //-------- Navigation Properties ---

    [ForeignKey("ProjectId")]
    public  Project Project { get; set; } = null!;

    public  ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
