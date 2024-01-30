using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<BranchesBarChartViewModel>> GetAllBranchesBarChartDataAsync();
    }
}
