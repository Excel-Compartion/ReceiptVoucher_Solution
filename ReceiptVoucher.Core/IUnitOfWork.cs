using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Identity;
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

        IProjectRepository Projects { get; }
        IBaseRepository<ApplicationUser> Users { get; }
        IBaseRepository<CompanyInfo> CompanyInfo { get; }
        //IBaseRepository<Receipt> Receipts { get; }
        ISubProjectRepository SubProjects { get; }

        IReceiptRepository Receipts { get; }

        void Detach<T>(T entity ) where T : class;

        int  Complete();

        Task<int> CompleteAsync();
    }
}
