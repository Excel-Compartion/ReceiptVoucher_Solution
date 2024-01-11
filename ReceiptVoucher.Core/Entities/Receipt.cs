namespace ReceiptVoucher.Core.Entities;

public class Receipt
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string ReceivedFrom { get; set; } = null!;
    [Required]
    [StringLength(10)]
    public string? ReceivedBy { get; set; }     // Foreign Key From User

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    public int BranchId { get; set; }   // Foreign Key

    [Column("Sub_ProjectId")]
    public int SubProjectId { get; set; }   // Foreign Key

    [StringLength(500)]
    public string ForPurpose { get; set; } = null!;

    public DateOnly Date { get; set; }

    // Payment Type specif Prop
    public string PaymentType { get; set; }= null!;

    public int CheckNumber { get; set; }

    public DateOnly CheckDate { get; set; }

    public int AccountNumber { get; set;}

    public string Bank { get; set; }=null!;









    //-------- Navigation Properties ---
    [ForeignKey("BranchId")]
    public Branch? Branch { get; set; } = null!;

    [ForeignKey("SubProjectId")]
    public SubProject? SubProject { get; set; } = null!;
}
