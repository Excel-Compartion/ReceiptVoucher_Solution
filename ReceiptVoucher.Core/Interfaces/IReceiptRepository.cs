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
           Expression<Func<Receipt, object>> orderBy = null, string orderByDirection = OrderBy.Decending, bool NoPagination = false,bool OrderByNumber=true,int? UserBranchId= null);

        Task<IEnumerable<GetReceiptDto>> GetFilteredData(ReceiptWithFilter_VM receiptWithFilter_VM);

        //  GetReceiptRdcl

        Task<Receipt>  GetReceiptRdclById(string code);

        Task<Receipt> GetLastAsync();

        Task<Receipt> GetLastReceiptWitheBranchAsync(int branchId);

    }
}
