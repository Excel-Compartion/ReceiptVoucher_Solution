using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Enum
{
   



        public enum GrantDest
        {
            [Display(Name = "فرد")]
            Individual = 1,
            [Display(Name = "شركه")]
            Company = 2,
            [Display(Name = "جمعية")]
            Association = 3
        }






    
}
