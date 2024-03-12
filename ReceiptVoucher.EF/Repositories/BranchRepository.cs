using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public async Task<IEnumerable<GetBranchDto>> GetAllBranchAsyncV2(Expression<Func<Branch, bool>> criteria, int? PageSize, int? PageNumber, string? search, Expression<Func<Branch, object>> orderBy = null, string orderByDirection = "DESC", bool NoPagination = false)
        {
            IQueryable<Branch> query = _context.Set<Branch>().AsNoTracking();

            if (criteria != null)
            {
                query = query.Where(criteria);
            }



            if (PageNumber.HasValue && PageSize.HasValue && NoPagination == false)
                query = query.Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);



            var Items = await query.Select(a => new GetBranchDto
            {
               Id=a.Id,
               Name=a.Name,
                Area=a.Area,
                Mobile=a.Mobile,
                Email = a.Email,
                ResponsiblePerson = a.ResponsiblePerson,
                IsActive = a.IsActive,
                CreatedDate = a.CreatedDate,
                AccountNumber = a.AccountNumber
            })
                .ToListAsync();


            return Items;
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
