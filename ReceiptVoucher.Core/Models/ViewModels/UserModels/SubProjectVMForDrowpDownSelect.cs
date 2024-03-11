using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels.UserModels
{
    public class SubProjectVMForDrowpDownSelect
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int ProjectId { get; set; }

        public string? ProjectName { get; set;}
    }
}
