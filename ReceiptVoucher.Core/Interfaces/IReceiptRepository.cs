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
    public interface IReceiptRepository: IBaseRepository<Receipt>
    {
        Task<IEnumerable<Receipt>> GetAllReceiptAsync();
        Task<IEnumerable<GetReceiptDto>> GetAllReceiptAsyncV2(Expression<Func<Receipt, bool>> criteria, int? PageSize, int? PageNumber, string? search,
           Expression<Func<Receipt, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<Receipt>> GetFilteredData( FilterData filterData);

        //  GetReceiptRdcl

        Task<Receipt>  GetReceiptRdclById(string code);

        Task<Receipt> GetLastAsync();

    }
}
