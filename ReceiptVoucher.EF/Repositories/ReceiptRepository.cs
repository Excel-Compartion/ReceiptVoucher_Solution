using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Models.Dtos;
using ReceiptVoucher.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.EF.Repositories
{
    public class ReceiptRepository : BaseRepository<Receipt>, IReceiptRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        public ReceiptRepository(ReceiptVoucherDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receipt>> GetAllReceiptAsync()
        {
            return await _context.Receipts.Include(p => p.Branch).Include(p=>p.SubProject).ToListAsync();
        }

        public async Task<Receipt> GetLastAsync()
        {
            return await _context.Receipts.OrderByDescending(r => r.Number).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Receipt>> GetFilteredData(FilterData filterData)
        {
            if (filterData.RadioDateType == "Day")
            {

                return await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project).Where(x => filterData.SelectProject.Contains(x.Project.Name) && filterData.SelectSubProject.Contains(x.SubProject.Name) && filterData.SelectBranchId.Contains(x.Branch.Name) && x.Date == filterData.SelectedDate).ToListAsync();


            }

            else
            {
                return await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project).Where(x => filterData.SelectProject.Contains(x.Project.Name) && filterData.SelectSubProject.Contains(x.SubProject.Name) && filterData.SelectBranchId.Contains(x.Branch.Name) && (x.Date >= filterData.SelectedMonth1 && x.Date <= filterData.SelectedMonth2)).ToListAsync();

            }
        }

        public async Task<Receipt>   GetReceiptRdclById(string code)
        {
            var Receipt = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Where(x => x.Code == code).FirstOrDefaultAsync();
           

            return Receipt;
        }

      
    }
}
