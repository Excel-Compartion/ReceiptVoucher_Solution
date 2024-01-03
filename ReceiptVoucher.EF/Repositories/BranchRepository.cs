using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptVoucher.Core.Entities;

namespace ReceiptVoucher.EF.Repositories
{
    public class BranchRepository : BaseRepository<Branch> , IBranchRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        public BranchRepository(ReceiptVoucherDbContext context) : base(context) 
        {
            
        }

        public Branch CreateBranch(Branch branch)
        {
            throw new NotImplementedException();
        }
    }
}
