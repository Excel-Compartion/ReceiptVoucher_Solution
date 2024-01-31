using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ReceiptVoucher.Core.Entities;

namespace ReceiptVoucher.EF.Repositories
{
    public class BranchRepository : BaseRepository<Branch> , IBranchRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        public BranchRepository(ReceiptVoucherDbContext context) : base(context) 
        {
            _context = context;
        }

       

        public async Task<bool> DeleteBranchAsync(int id)
        {
            var isDeleted = false;

            var branch = await _context.Branches.FindAsync(id);

            if (branch is null)
                return false;


            var receipt = await _context.Receipts.Where(x => x.BranchId == id).ToListAsync();

            if(receipt !=null && receipt.Count() > 0)
            {
                return false;
            }

            _context.Branches.Remove(branch);

            int effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }
    }
}
