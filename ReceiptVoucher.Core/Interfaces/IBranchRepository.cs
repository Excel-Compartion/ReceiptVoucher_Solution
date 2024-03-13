using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.Dtos;
using ReceiptVoucher.Core.Models.ViewModels;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {


        Task<List<BranchVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync();
        Task<bool> DeleteBranchAsync(int id);

        Task<IEnumerable<GetBranchDto>> GetAllBranchAsyncV2(Expression<Func<Branch, bool>> criteria, int? PageSize, int? PageNumber, string? search,
           Expression<Func<Branch, object>> orderBy = null, string orderByDirection = OrderBy.Decending, bool NoPagination = false);

    }
}
