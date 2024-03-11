using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Models.ViewModels;
using ReceiptVoucher.Core.Models.ViewModels.UserModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface ISubProjectRepository : IBaseRepository<SubProject>
    {
        Task<IEnumerable<SubProject>> GetAllSubProjectAsync();

        Task<bool> DeleteSubProjectAsync(int id);

        Task<List<SubProjectVMForDrowpDownSelect>> GetAllForDrowpDownSelectAsync();
    }
}
