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

        public async Task<SubProject> AddSubProjectAsync(SubProject subProject)
        {

            // تأكد من أن ProjectId يشير إلى مشروع موجود
            var project = await _context.Projects.FindAsync(subProject.ProjectId);
            if (project == null)
            {
                return subProject;
            }
            else
            {
                // تعيين الكائن Project
                subProject.Project = project;

                _context.SubProjects.Add(subProject);
                await _context.SaveChangesAsync();


                return subProject;
            }
           
           
        }


    }
}
