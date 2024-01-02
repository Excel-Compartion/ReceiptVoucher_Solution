using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {
        // here i write my spcial methods for Entity as Signuture

        Branch CreateBranch(Branch branch);

    }
}
