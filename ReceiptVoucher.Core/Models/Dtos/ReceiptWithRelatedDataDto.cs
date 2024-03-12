using ReceiptVoucher.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.Dtos
{
    public class ReceiptWithRelatedDataDto
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

        public string? BranchName { get; set; }


        public string? ProjectName { get; set; }

      

        public string? SubProjectName { get; set; }

        [StringLength(500)]
        public string? ForPurpose { get; set; }

        public DateOnly Date { get; set; }


        public string? DateToString =>Date + "م  ";




        public string? GrantDestinationName { get; set; } 


       

        public string? GenderName { get; set; }

       

        public string? AgeName { get; set; }

        public string? Mobile { get; set; }

        // Payment Type  Prop Related

        

        public string? PaymentTypeName { get; set; }


        public int? CheckNumber { get; set; }


        //public DateOnly? CheckDate { get; set; }

        //public string? CheckDateToString =>CheckDate +"";


        public int? AccountNumber { get; set; }

        [MinLength(2)]
        public string? Bank { get; set; }


        public string Code { get; set; } = null!;


        public int Number { get; set; }

        public int ReceiptBranchNumber { get; set; }

        public string? ReceiptBranchNumWithBranchNum { get; set; }
    }
}
