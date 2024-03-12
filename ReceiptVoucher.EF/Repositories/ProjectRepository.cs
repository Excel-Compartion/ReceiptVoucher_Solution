using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.Dtos;
using ReceiptVoucher.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReceiptVoucher.EF.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(ReceiptVoucherDbContext context , IMapper mapper) : base(context ,mapper )
        {
            _context = context;
            this._mapper = mapper;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var isDeleted = false;

            var project = await _context.Projects.FindAsync(id);

            if (project is null)
                return false;


            var receipt = await _context.Receipts.Where(x => x.ProjectId == id).ToListAsync();

            var subProject= await _context.SubProjects.Where(x => x.ProjectId == id).ToListAsync();

            if ((receipt != null && receipt.Count() > 0)|| (subProject != null && subProject.Count() > 0))
            {
                return false;
            }

            _context.Projects.Remove(project);

            int effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
                isDeleted = true;

            return isDeleted;
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            // بدء وحدة تحكم في المعاملات
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // إذا كان المشروع غير نشط، قم بتعديل جميع المشاريع الفرعية المرتبطة بالمشروع لتكون غير نشطة أيضا
                if (!project.IsActive)
                {
                    var subProjects = _context.SubProjects.Where(sp => sp.ProjectId == project.Id);
                    foreach (var subProject in subProjects)
                    {
                        subProject.IsActive = false;
                        _context.SubProjects.Update(subProject);
                    }
                }
                else if (project.IsActive)
                {
                    var subProjects = _context.SubProjects.Where(sp => sp.ProjectId == project.Id);
                    foreach (var subProject in subProjects)
                    {
                        subProject.IsActive = true;
                        _context.SubProjects.Update(subProject);
                    }
                }

                // تحديث البيانات الأساسية للمشروع
                _context.Projects.Update(project);

                // حفظ التغييرات
                await _context.SaveChangesAsync();

                // التأكيد على وحدة تحكم في المعاملات
                await transaction.CommitAsync();

                // إذا تم التحديث بنجاح، قم بإرجاع true
                return true;
            }
            catch (Exception)
            {
                // إذا حدث خطأ، ستتم إعادة جميع التغييرات
                await transaction.RollbackAsync();

                // أعد رمي الاستثناء للتعامل معه في مكان آخر
                // إذا حدث خطأ، قم بإرجاع false
                return false;
            }
        }

        public async Task<List<ProjectVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync()
        {
            IQueryable<Project> query = _context.Set<Project>().AsNoTracking();

            var Items = await query.Select(a => new ProjectVMForDrowpDownSelect
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync();


            return Items;

        }

        public async Task<IEnumerable<GetProjectDto>> GetAllProjectAsyncV2(Expression<Func<Project, bool>> criteria, int? PageSize, int? PageNumber, string? search, Expression<Func<Project, object>> orderBy = null, string orderByDirection = "DESC", bool NoPagination = false)
        {
            IQueryable<Project> query = _context.Set<Project>().AsNoTracking();

            if (criteria != null)
            {
                query = query.Where(criteria);
            }



            if (PageNumber.HasValue && PageSize.HasValue && NoPagination == false)
                query = query.Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);



            var Items = await query.Select(a => new GetProjectDto
            {
                Id = a.Id,
                Name = a.Name,
                Note=a.Note,
                IsActive=a.IsActive,
                Date = a.Date

            })
                .ToListAsync();


            return Items;
        }
    }
}
