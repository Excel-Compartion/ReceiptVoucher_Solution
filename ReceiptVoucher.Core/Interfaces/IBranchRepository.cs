using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.ViewModels;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {


        Task<List<BranchVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync();
        Task<bool> DeleteBranchAsync(int id);

    }
}
