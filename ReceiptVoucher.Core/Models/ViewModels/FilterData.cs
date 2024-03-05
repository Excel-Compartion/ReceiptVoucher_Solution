using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels
{
    public class FilterData
    {
        public string RadioDateType { get; set; }
        public DateOnly? SelectedDate { get; set; }
        public DateOnly? SelectedMonth1 { get; set; }
        public DateOnly? SelectedMonth2 { get; set; }
        public List<string> SelectProject { get; set; }
        public List<string> SelectSubProject { get; set; }
        public List<string> SelectBranchId { get; set; }
        public List<string> SelectGrantDestinations { get; set; }
        public List<string> SelectPaymentTypes { get; set; }
        public int? UserBranchId { get; set; }
    }
}
