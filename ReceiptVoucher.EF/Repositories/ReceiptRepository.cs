using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Enums;
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
        public async Task<Receipt>   GetReceiptRdclById(int id)
        {
            var Receipt = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Where(x => x.Id == id).FirstOrDefaultAsync();

           

            return Receipt;
        }

      
    }
}
