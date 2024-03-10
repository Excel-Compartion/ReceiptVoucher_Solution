using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Models.Dtos;
using ReceiptVoucher.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace ReceiptVoucher.EF.Repositories
{
    public class ReceiptRepository : BaseRepository<Receipt>, IReceiptRepository
    {
        private readonly ReceiptVoucherDbContext _context;
        private readonly IMapper mapper;
        public ReceiptRepository(ReceiptVoucherDbContext context, IMapper _mapper) : base(context)
        {
            _context = context;
            mapper = _mapper;
        }

        public async Task<IEnumerable<Receipt>> GetAllReceiptAsync()
        {
            return await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).ToListAsync();
        }

        public async Task<Receipt> GetLastAsync()
        {
            return await _context.Receipts.OrderByDescending(r => r.Number).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Receipt>> GetFilteredData(FilterData filterData)
        {

            ////////////////////////   معرفة عنصر ال enum هل هو موجود في القائمه المحدده //////////////////////////////

            List<PaymentTypes> V_PaymentTypes = new List<PaymentTypes>();

            List<GrantDest> V_GrantDestinations = new List<GrantDest>();

            Dictionary<string, PaymentTypes> Dic_payments = new Dictionary<string, PaymentTypes>();

            Dictionary<string, GrantDest> Dic_grantDestinations = new Dictionary<string, GrantDest>();

            Dic_payments[PaymentTypes.Account.GetDisplayName()] = PaymentTypes.Account;
            Dic_payments[PaymentTypes.Bank.GetDisplayName()] = PaymentTypes.Bank;
            Dic_payments[PaymentTypes.Cash.GetDisplayName()] = PaymentTypes.Cash;
            Dic_payments[PaymentTypes.Check.GetDisplayName()] = PaymentTypes.Check;




            foreach (var PaymentType in filterData.SelectPaymentTypes)
            {
                if (Dic_payments.ContainsKey(PaymentType))
                {
                    V_PaymentTypes.Add(Dic_payments[PaymentType]);
                }

            }


            Dic_grantDestinations[GrantDest.Company.GetDisplayName()] = GrantDest.Company;
            Dic_grantDestinations[GrantDest.Association.GetDisplayName()] = GrantDest.Association;
            Dic_grantDestinations[GrantDest.Foundation.GetDisplayName()] = GrantDest.Foundation;
            Dic_grantDestinations[GrantDest.Individual.GetDisplayName()] = GrantDest.Individual;




            foreach (var grantDest in filterData.SelectGrantDestinations)
            {
                if (Dic_grantDestinations.ContainsKey(grantDest))
                {
                    V_GrantDestinations.Add(Dic_grantDestinations[grantDest]);
                }

            }

            //////////////////////////////////////////////////////////////////////////////////////////////////




            bool IsFilter = filterData.SelectGrantDestinations.Count > 0
         && filterData.SelectPaymentTypes.Count > 0
         && filterData.SelectProject.Count > 0
         && filterData.SelectSubProject.Count > 0
         && (filterData.SelectBranchId.Count > 0 || filterData.UserBranchId != null);



            if (filterData.RadioDateType == "Day")
            {


                if (IsFilter)
                {
                    if (filterData.UserBranchId == null)
                    {
                        var receipts = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project)
                        .Where(x => filterData.SelectProject.Contains(x.Project.Name) && filterData.SelectSubProject.Contains(x.SubProject.Name)
                         && filterData.SelectBranchId.Contains(x.Branch.Name) && x.Date == filterData.SelectedDate
                         && V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)
                         ).ToListAsync();

                        return receipts;
                    }
                    else
                    {
                        var receipts = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project)
                       .Where(x => filterData.SelectProject.Contains(x.Project.Name) && filterData.SelectSubProject.Contains(x.SubProject.Name)
                        && x.BranchId == filterData.UserBranchId && x.Date == filterData.SelectedDate
                        && V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)
                        ).ToListAsync();

                        return receipts;
                    }

                }

                else
                {
                    if (filterData.UserBranchId == null)
                    {
                        var receipts = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project)
                       .Where(x => x.Date == filterData.SelectedDate).ToListAsync();
                        return receipts;
                    }
                    else
                    {
                        var receipts = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project)
                      .Where(x => x.Date == filterData.SelectedDate && x.BranchId == filterData.UserBranchId).ToListAsync();
                        return receipts;
                    }

                }



            }

            else
            {
                if (IsFilter)
                {
                    if (filterData.UserBranchId == null)
                    {
                        var receipts = await _context.Receipts
                       .Include(p => p.Branch)
                       .Include(p => p.SubProject)
                       .Include(p => p.Project)
                       .Where(x =>
                       filterData.SelectProject.Contains(x.Project.Name) &&
                       filterData.SelectSubProject.Contains(x.SubProject.Name) &&
                       filterData.SelectBranchId.Contains(x.Branch.Name) &&
                       (x.Date >= filterData.SelectedMonth1 && x.Date <= filterData.SelectedMonth2) &&
                       V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)).ToListAsync();

                        return receipts;
                    }

                    else
                    {
                        var receipts = await _context.Receipts
                      .Include(p => p.Branch)
                      .Include(p => p.SubProject)
                      .Include(p => p.Project)
                      .Where(x =>
                      filterData.SelectProject.Contains(x.Project.Name) &&
                      filterData.SelectSubProject.Contains(x.SubProject.Name) &&
                     x.BranchId==filterData.UserBranchId &&
                      (x.Date >= filterData.SelectedMonth1 && x.Date <= filterData.SelectedMonth2) &&
                      V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)).ToListAsync();

                        return receipts;
                    }

                }

                else
                {
                    if (filterData.UserBranchId == null)
                    {
                        var receipts = await _context.Receipts
                       .Include(p => p.Branch)
                       .Include(p => p.SubProject)
                       .Include(p => p.Project)
                       .Where(x =>
                       (x.Date >= filterData.SelectedMonth1 && x.Date <= filterData.SelectedMonth2))
                       .ToListAsync();

                        return receipts;
                    }

                    else
                    {
                        var receipts = await _context.Receipts
                       .Include(p => p.Branch)
                       .Include(p => p.SubProject)
                       .Include(p => p.Project)
                       .Where(x =>
                       (x.Date >= filterData.SelectedMonth1 && x.Date <= filterData.SelectedMonth2) &&x.BranchId==filterData.UserBranchId)
                       .ToListAsync();

                        return receipts;
                    }
                    
                }

            }
        }

        public async Task<Receipt> GetReceiptRdclById(string code)
        {
            var Receipt = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Where(x => x.Code == code).FirstOrDefaultAsync();


            return Receipt;
        }

        public async Task<IEnumerable<GetReceiptDto>> GetAllReceiptAsyncV2(Expression<Func<Receipt, bool>> criteria, int? PageSize, int? PageNumber, string? search,
           Expression<Func<Receipt, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<Receipt> query = _context.Set<Receipt>();
            //if (!string.IsNullOrWhiteSpace(search)) 
            //{
            //    query = query.Where(r =>
            //        r.GetType().GetProperties()
            //        .Any(p => p.PropertyType == typeof(string) &&
            //        p.GetValue(r).ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true)
            //        );
            //}

            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            if (PageNumber.HasValue && PageSize.HasValue)
                query = query.Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            IEnumerable<GetReceiptDto> Items = mapper.Map<List<GetReceiptDto>>(query);

            return  Items;


            //var receipts = await _context.Receipts.Select(a => new GetReceiptDto
            //{
            //    Id = a.Id,
            //    AccountNumber =a.AccountNumber,
            //    Bank = a.Bank,
            //    BranchName = a.Branch.Name,
            //    CheckDate =a.CheckDate,
            //    Code =a.Code,
            //    Date =a.Date,
            //    ForPurpose =a.ForPurpose,
            //    Mobile =a.Mobile,
            //    Number =a.Number,
            //    ProjectName = a.Project.Name,
            //    ReceivedByName = a.ReceivedBy,
            //    TotalAmount = a.TotalAmount,
            //    SubProjectName = a.SubProject.Name,
            //    CheckNumber = a.CheckNumber,
            //    ReceivedBy = a.ReceivedBy,
            //    GrantDestinations = a.GrantDestinations,
            //    PaymentType = a.PaymentType,
            //    Age = a.Age,
            //    Gender = a.Gender,
            //    ReceivedFrom = a.ReceivedFrom,
            //    BranchId=a.BranchId,
            //    ProjectId= a.ProjectId,
            //    SubProjectId= a.SubProjectId
                
                
            //}).ToListAsync();
           
        }
    }
}
