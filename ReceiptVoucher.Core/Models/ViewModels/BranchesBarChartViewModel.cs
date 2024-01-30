using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class BranchesBarChartViewModel
    {

        public double[] Individual { get; set; } = null!;

        public double[] Company { get; set; } = null!;

        public double[] Association { get; set; } = null!;

        public double[] Foundation { get; set; }= null!;

        public string[] BranchesNames { get; set; } = null!;
    }
}
