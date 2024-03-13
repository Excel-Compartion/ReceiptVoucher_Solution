using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.Dtos;
using ReceiptVoucher.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {

        Task<bool> UpdateProjectAsync(Project project);

        Task<bool> DeleteProjectAsync(int id);

        Task<List<ProjectVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync();

        Task<IEnumerable<GetProjectDto>> GetAllProjectAsyncV2(Expression<Func<Project, bool>> criteria, int? PageSize, int? PageNumber, string? search,
         Expression<Func<Project, object>> orderBy = null, string orderByDirection = OrderBy.Decending, bool NoPagination = false);
    }

}
