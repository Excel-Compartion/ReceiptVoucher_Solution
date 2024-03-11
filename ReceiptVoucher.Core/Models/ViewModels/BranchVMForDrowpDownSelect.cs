using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class BranchVMForDrowpDownSelect
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int AccountNumber { get; set; }
    }
}
