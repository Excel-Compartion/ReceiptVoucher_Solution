using ReceiptVoucher.Core.Entities;
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

        public async Task<IEnumerable<Receipt>> GetAllSubProjectAsync()
        {
            return await _context.Receipts.Include(p => p.Branch).Include(p=>p.SubProject).ToListAsync();
        }
    }
}
