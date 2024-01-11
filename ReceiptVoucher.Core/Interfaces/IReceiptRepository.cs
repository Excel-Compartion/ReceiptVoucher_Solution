using ReceiptVoucher.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface IReceiptRepository: IBaseRepository<Receipt>
    {
        Task<IEnumerable<Receipt>> GetAllSubProjectAsync();
    }
}
