using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.EF.Repositories
{
    public class ReportRepository:  IReportRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        public ReportRepository(ReceiptVoucherDbContext context) 
        {
            _context = context;
        }

        public Task<IEnumerable<BranchesBarChartViewModel>> GetAllBranchesBarChartDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
