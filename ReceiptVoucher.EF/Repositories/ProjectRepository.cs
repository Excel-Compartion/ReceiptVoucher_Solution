using ReceiptVoucher.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.EF.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly ReceiptVoucherDbContext _context;

        public ProjectRepository(ReceiptVoucherDbContext context) : base(context)
        {
            _context = context;
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



    }
}
