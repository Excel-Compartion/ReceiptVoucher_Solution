using ReceiptVoucher.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.Dtos
{
    public class GetReceiptDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المودع")]
        [StringLength(250)]
        public string ReceivedFrom { get; set; } = null!;


        [Required]
        public string ReceivedBy { get; set; }     // Foreign Key From User

        public string? ReceivedByName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public string TotalAmountToString => TotalAmount + "ريال  ";


        [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المكتب  ---")]
        public int BranchId { get; set; }   // Foreign Key

        public string? BranchName { get; set; }

        public int? BranchNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المشروع الرئيسي ---")]
        [Column("ProjectId")]
        public int ProjectId { get; set; }   // Foreign Key

        public string? ProjectName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المشروع الفرعي ---")]
        [Column("Sub_ProjectId")]
        public int SubProjectId { get; set; }   // Foreign Key

        public string? SubProjectName { get; set; }

        [StringLength(500)]
        public string? ForPurpose { get; set; }

        public DateOnly Date { get; set; }
        

        public GrantDest GrantDestinations { get; set; }

        public string? GrantDestinationName => GrantDestinations.GetDisplayName();


        public Gender Gender { get; set; }

        public string? GenderName =>Gender!=0 ? Gender.GetDisplayName():null;

        public Age Age { get; set; }

        public string? AgeName => Age != 0 ? Age.GetDisplayName() : null;

        public string? Mobile { get; set; }

        // Payment Type  Prop Related

        public PaymentTypes PaymentType { get; set; }

        public string? PaymentTypeName => PaymentType.GetDisplayName();

        public int? CheckNumber { get; set; }


        public DateOnly? CheckDate { get; set; }


        public int? AccountNumber { get; set; }

        [MinLength(2)]
        public string? Bank { get; set; }


        public string Code { get; set; } = null!;


        public int Number { get; set; }


    }

    public class PostReceiptDto
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

       


        [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار اسم المكتب  ---")]
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


        public int? AccountNumber { get; set; }

        [MinLength(2)]
        public string? Bank { get; set; }


        public string Code { get; set; } = null!;


        public int Number { get; set; }
    }
}
