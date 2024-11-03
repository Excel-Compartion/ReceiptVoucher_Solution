using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.ViewModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.EF.Repositories
{
    public class SubProjectRepository : BaseRepository<SubProject>, ISubProjectRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        private readonly IMapper _mapper;

        public SubProjectRepository(ReceiptVoucherDbContext context , IMapper mapper) : base(context , mapper)
        {
            _context = context;
            this._mapper = mapper;
        }
    
        public async Task<IEnumerable<SubProject>> GetAllSubProjectAsync()
        {
            return await _context.SubProjects.Include(p => p.Project).ToListAsync();
        }

        public async Task<bool> DeleteSubProjectAsync(int id)
        {
            var isDeleted = false;

            var subProject = await _context.SubProjects.FindAsync(id);

            if (subProject is null)
                return false;


            var receipt = await _context.Receipts.Where(x => x.SubProjectId == id).ToListAsync();

            if (receipt != null && receipt.Count() > 0)
            {
                return false;
            }

            _context.SubProjects.Remove(subProject);

            int effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }


        public async Task<List<SubProjectVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync()
        {
            IQueryable<SubProject> query = _context.Set<SubProject>().AsNoTracking();

            var Items = await query.Where(x=>x.IsActive).Select(a => new SubProjectVMForDrowpDownSelect
            {
                Id = a.Id,
                Name = a.Name,
                ProjectId=a.ProjectId,
                ProjectName=a.Project.Name

            }).ToListAsync();


            return Items;

        }

    }
}
