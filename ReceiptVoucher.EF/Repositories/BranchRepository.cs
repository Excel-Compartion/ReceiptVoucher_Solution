using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.Dtos;
using ReceiptVoucher.Core.Models.ViewModels;

namespace ReceiptVoucher.EF.Repositories
{
    public class BranchRepository : BaseRepository<Branch> , IBranchRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        private readonly IMapper _mapper;

        public BranchRepository(ReceiptVoucherDbContext context, IMapper mapper) : base(context , mapper) 
        {
            _context = context;
            this._mapper = mapper;
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

        public async Task<List<BranchVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync()
        {
            IQueryable<Branch> query = _context.Set<Branch>().AsNoTracking();

            var Items = await query.Select(a => new BranchVMForDrowpDownSelect
            {
                Id = a.Id,
                Name = a.Name ,
                AccountNumber=a.AccountNumber
                
            }).ToListAsync();


            return Items;

        }
    }
}
