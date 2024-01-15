using ReceiptVoucher.Core.Entities;
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
    }
}
