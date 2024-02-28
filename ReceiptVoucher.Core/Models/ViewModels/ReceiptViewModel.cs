using ReceiptVoucher.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class ReceiptViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم المودع")]
        [StringLength(250)]
        public string ReceivedFrom { get; set; } = null!;


        public string ReceivedBy { get; set; }      // Foreign Key From User


        public string? ReceivedByName { get; set; }

        [Range(1.0, 9999999999999999.0, ErrorMessage = "--- يرجى ادخال اجمالي المبلغ بشكل صحيح ---")]
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

        // Payment Type  Prop Related

        public PaymentTypes PaymentType { get; set; }

        [Required(ErrorMessage = "يرجى ادخال رقم الشيك بشكل صحيح ")]

        public int? CheckNumber { get; set; }

        [Required(ErrorMessage = "يرجى ادخال تاريخ ")]
        public DateTime? CheckDate { get; set; }

        [Required(ErrorMessage = "يرجى ادخال رقم نقطة البيع بشكل صحيح ")]
       
        public int? AccountNumber { get; set; }

        [Required(ErrorMessage = "يرجى ادخال اسم البنك")]
        [MinLength(2)]
        public string? Bank { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "--- يرجى اختيار الجهه المانحه  ---")]
        public GrantDest GrantDestinations { get; set; }

        
        public Gender Gender { get; set; }

        
        [Range(1, int.MaxValue, ErrorMessage = "يرجى تحديد العمر")]
        public Age Age { get; set; }

        
        public string? Mobile { get; set; }



        public string Code { get; set; } = null!;


        public int Number { get; set; }


    }
}
