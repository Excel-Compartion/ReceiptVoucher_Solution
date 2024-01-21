

using ReceiptVoucher.Core.Enums;


namespace ReceiptVoucher.Core.Entities;

public class Receipt
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "يرجى ادخال اسم المودع")]
    [StringLength(250)]
    public string ReceivedFrom { get; set; } = null!;

    [Required(ErrorMessage ="يرجى ادخال اسم المحاسب")]
    [StringLength(250)]
    public string? ReceivedBy { get; set; }     // Foreign Key From User

    [Range(1.0, int.MaxValue, ErrorMessage = "--- يرجى ادخال اجمالي المبلغ ---")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }


    [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم الفرع  ---")]
    public int BranchId { get; set; }   // Foreign Key

    [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المشروع الفرعي ---")]
    [Column("Sub_ProjectId")]
    public int SubProjectId { get; set; }   // Foreign Key

    [StringLength(500)]
    public string ForPurpose { get; set; } = null!;

    public DateOnly Date { get; set; }




  
    public GrantDest GrantDestinations { get; set; }

    
    public Gender Gender { get; set; }

    public Age Age { get; set; }

    public int? Mobile { get; set; }

    


    


    // Payment Type  Prop Related

    public PaymentTypes PaymentType { get; set; }


    
    [Range(100000, 9999999999999999, ErrorMessage = "يرجى ادخال رقم الشيك بين 6 - 16 عدد")]
    public int? CheckNumber { get; set; }


    public DateOnly? CheckDate { get; set; }

    [Range(100000, 9999999999999999, ErrorMessage = "يرجى ادخال رقم الحساب بين 6 - 16 عدد")]
    public int? AccountNumber { get; set;}

    [MinLength(2)]
    public string? Bank { get; set; }



    //-------- Navigation Properties ---
    [ForeignKey("BranchId")]
    public Branch? Branch { get; set; } = null!;

    [ForeignKey("SubProjectId")]
    public SubProject? SubProject { get; set; } = null!;
}
