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

        public async Task<DTO> AddSubProjectAsync(DTO dTO)
        {

            // تأكد من أن ProjectId يشير إلى مشروع موجود
            var project = await _context.Projects.FindAsync(dTO.ProjectId);
            if (project == null)
            {
                return dTO;
            }
            else
            {
                SubProject subProject = new SubProject
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Duration = dTO.Duration,
                    ProjectId = dTO.ProjectId,
                    Name = dTO.Name,

                };

                _context.SubProjects.Add(subProject);
                await _context.SaveChangesAsync();


                return dTO;
            }
           
           
        }

        public async Task<DTO> UpdateSubProjectAsync(DTO dTO)
        {
            var subProject =await _context.SubProjects.FindAsync(dTO.SubProjectId);

            if (subProject != null)
            {
                subProject.Name= dTO.Name;
                subProject.Duration=dTO.Duration;
                subProject.ProjectId=dTO.ProjectId;

                _context.Update(subProject);
                await _context.SaveChangesAsync();

            }

           
            return dTO;
        }
    }
}
