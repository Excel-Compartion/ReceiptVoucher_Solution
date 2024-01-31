using ReceiptVoucher.Core.Entities;
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
    }
}
