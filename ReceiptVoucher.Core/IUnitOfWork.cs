using ReceiptVoucher.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core
{
    public interface IUnitOfWork : IDisposable
    {
        //IBaseRepository<Branch> Branches { get; }
        IBranchRepository Branches { get; }

        IBaseRepository<Project> Projects { get; }
        IBaseRepository<CompanyInfo> CompanyInfo { get; }
        IBaseRepository<Receipt> Receipts { get; }
        IBaseRepository<SubProject> SubProjects { get; }

        int Complete();
    }
}
