using ReceiptVoucher.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class ReceiptRdclViewModel
    {
        [Key]
        public int Id { get; set; }

        
        public string? ReceivedFrom { get; set; } = null!;

        
        public string? ReceivedBy { get; set; }     // Foreign Key From User


        
        public string? TotalAmount { get; set; }


       
        public string? Branch { get; set; }   // Foreign Key

       
        public string? SubProject { get; set; }   // Foreign Key

       
        public string? ForPurpose { get; set; } 

        public string? Date { get; set; }

       

        public string? PaymentType { get; set; }

       
        public string? CheckNumber { get; set; }

       
        public string? CheckDate { get; set; }

       
        public string? AccountNumber { get; set; }

       
        public string? Bank { get; set; }


       
        public string? GrantDestinations { get; set; }


        public string? Gender { get; set; }


       
        public string? Age { get; set; }


        public string? Mobile { get; set; }

        public string? DateArabic { get; set; }

        public string? TotalAmountWord { get; set; }

        public string? Number { get; set; }

        public string? ReceiptBranchNumWithBranchNum { get; set; }
    }
}
