

using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Identity;


namespace ReceiptVoucher.Core.Entities;

public class Receipt
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "يرجى ادخال اسم المودع")]
    [StringLength(250)]
    public string ReceivedFrom { get; set; } = null!;


    [Required]
    public string ReceivedBy { get; set; }     // Foreign Key From User

  
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }


    [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم الفرع  ---")]
    public int BranchId { get; set; }   // Foreign Key

    [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المشروع الرئيسي ---")]
    [Column("ProjectId")]
    public int ProjectId { get; set; }   // Foreign Key

    [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المشروع الفرعي ---")]
    [Column("Sub_ProjectId")]
    public int SubProjectId { get; set; }   // Foreign Key

    [StringLength(500)]
    public string? ForPurpose { get; set; }

    public DateOnly Date { get; set; }

  
    public GrantDest GrantDestinations { get; set; }

    
    public Gender Gender { get; set; }

    public Age Age { get; set; }

    public string? Mobile { get; set; }

    // Payment Type  Prop Related

    public PaymentTypes PaymentType { get; set; }


    public int? CheckNumber { get; set; }


    public DateOnly? CheckDate { get; set; }


    public int? AccountNumber { get; set;}

    [MinLength(2)]
    public string? Bank { get; set; }



    //-------- Navigation Properties ---
    [ForeignKey(nameof(ReceivedBy))]
    public ApplicationUser ApplicationUser { get; set; }


    [ForeignKey("BranchId")]
    public Branch? Branch { get; set; } = null!;

    [ForeignKey("ProjectId")]
    public Project? Project { get; set; } = null!;

    [ForeignKey("SubProjectId")]
    public SubProject? SubProject { get; set; } = null!;
}
