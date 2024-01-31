using ReceiptVoucher.Core.Entities;
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
        public SubProjectRepository(ReceiptVoucherDbContext context) : base(context)
        {
            _context = context;
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
    }
}
