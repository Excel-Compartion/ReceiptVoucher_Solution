using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models
{
    public class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = string.Empty;

        public bool NoPagination { get; set; } 
    }
}
