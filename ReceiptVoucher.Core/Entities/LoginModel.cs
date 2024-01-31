using ReceiptVoucher.Core.Models.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Entities
{
    public class LoginModel : TokenRequestModel
    {
        public bool? RemmberMe { get; set; }
    }
}
