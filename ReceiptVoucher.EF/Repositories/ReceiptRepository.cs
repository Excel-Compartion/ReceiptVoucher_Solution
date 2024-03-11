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

        public async Task<IEnumerable<GetReceiptDto>> GetFilteredData(ReceiptWithFilter_VM receiptWithFilter_VM)
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




            foreach (var PaymentType in receiptWithFilter_VM.SelectPaymentTypes)
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




            foreach (var grantDest in receiptWithFilter_VM.SelectGrantDestinations)
            {
                if (Dic_grantDestinations.ContainsKey(grantDest))
                {
                    V_GrantDestinations.Add(Dic_grantDestinations[grantDest]);
                }

            }

            //////////////////////////////////////////////////////////////////////////////////////////////////




            bool IsFilter = receiptWithFilter_VM.SelectGrantDestinations.Count > 0
         && receiptWithFilter_VM.SelectPaymentTypes.Count > 0
         && receiptWithFilter_VM.SelectProject.Count > 0
         && receiptWithFilter_VM.SelectSubProject.Count > 0
         && (receiptWithFilter_VM.SelectBranchId.Count > 0);

            IQueryable<Receipt> query = _context.Set<Receipt>().AsNoTracking();

            if (!receiptWithFilter_VM.NoPagination)
            {
                query = query.OrderByDescending(o => o.Number).Skip((receiptWithFilter_VM.PageNumber - 1) * receiptWithFilter_VM.PageSize).Take(receiptWithFilter_VM.PageSize);

            }



            if (receiptWithFilter_VM.RadioDateType == "Day")
            {


                if (IsFilter)
                {

                    //var receipts = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project)
                    //.Where(x => receiptWithFilter_VM.SelectProject.Contains(x.Project.Name) && receiptWithFilter_VM.SelectSubProject.Contains(x.SubProject.Name)
                    // && receiptWithFilter_VM.SelectBranchId.Contains(x.Branch.Name) && x.Date == receiptWithFilter_VM.SelectedDate
                    // && V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)
                    // ).ToListAsync();



                   

                    

                    var Items = await query
                        .Where(x => receiptWithFilter_VM.SelectProject.Contains(x.Project.Name) && receiptWithFilter_VM.SelectSubProject.Contains(x.SubProject.Name)
                         && receiptWithFilter_VM.SelectBranchId.Contains(x.Branch.Name) && x.Date == receiptWithFilter_VM.SelectedDate
                         && V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)
                         )
                        .Select(a => new GetReceiptDto
                        {
                            Id = a.Id,
                            AccountNumber = a.AccountNumber,
                            Bank = a.Bank,
                            BranchName = a.Branch.Name ,
                            CheckDate = a.CheckDate,
                            Code = a.Code,
                            Date = a.Date,
                            ForPurpose = a.ForPurpose,
                            Mobile = a.Mobile,
                            Number = a.Number,
                            ProjectName = a.Project.Name,
                            ReceivedByName = a.ReceivedBy,
                            TotalAmount = a.TotalAmount,
                            SubProjectName = a.SubProject.Name,
                            CheckNumber = a.CheckNumber,
                            ReceivedBy = a.ReceivedBy,
                            GrantDestinations = a.GrantDestinations,
                            PaymentType = a.PaymentType,
                            Age = a.Age,
                            Gender = a.Gender,
                            ReceivedFrom = a.ReceivedFrom,
                            BranchId = a.BranchId,
                            ProjectId = a.ProjectId,
                            SubProjectId = a.SubProjectId
                        })
                        .ToListAsync();

                    return Items;

                }

                else
                {

                    // var receipts = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Include(p => p.Project)
                    //.Where(x => x.Date == receiptWithFilter_VM.SelectedDate).ToListAsync();
                    // return receipts;


                   

                    

                    var Items = await query
                         .Where(x => x.Date == receiptWithFilter_VM.SelectedDate)
                        .Select(a => new GetReceiptDto
                        {
                            Id = a.Id,
                            AccountNumber = a.AccountNumber,
                            Bank = a.Bank,
                            BranchName = a.Branch.Name ?? "",
                            CheckDate = a.CheckDate,
                            Code = a.Code,
                            Date = a.Date,
                            ForPurpose = a.ForPurpose,
                            Mobile = a.Mobile,
                            Number = a.Number,
                            ProjectName = a.Project.Name,
                            ReceivedByName = a.ReceivedBy,
                            TotalAmount = a.TotalAmount,
                            SubProjectName = a.SubProject.Name,
                            CheckNumber = a.CheckNumber,
                            ReceivedBy = a.ReceivedBy,
                            GrantDestinations = a.GrantDestinations,
                            PaymentType = a.PaymentType,
                            Age = a.Age,
                            Gender = a.Gender,
                            ReceivedFrom = a.ReceivedFrom,
                            BranchId = a.BranchId,
                            ProjectId = a.ProjectId,
                            SubProjectId = a.SubProjectId
                        })
                        .ToListAsync();

                    return Items;




                }



            }

            else
            {
                if (IsFilter)
                {

                    // var receipts = await _context.Receipts
                    //.Include(p => p.Branch)
                    //.Include(p => p.SubProject)
                    //.Include(p => p.Project)
                    //.Where(x =>
                    //receiptWithFilter_VM.SelectProject.Contains(x.Project.Name) &&
                    //receiptWithFilter_VM.SelectSubProject.Contains(x.SubProject.Name) &&
                    //receiptWithFilter_VM.SelectBranchId.Contains(x.Branch.Name) &&
                    //(x.Date >= receiptWithFilter_VM.SelectedMonth1 && x.Date <= receiptWithFilter_VM.SelectedMonth2) &&
                    //V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations)).ToListAsync();

                    // return receipts;

                    Console.WriteLine("PaymentTypes in query:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.PaymentType);
                    }

                    Console.WriteLine("PaymentTypes in V_PaymentTypes:");
                    foreach (var item in V_PaymentTypes)
                    {
                        Console.WriteLine(item);
                    }

                    var tempQuery = query;

                    // Apply the first condition
                    tempQuery = tempQuery.Where(x => receiptWithFilter_VM.SelectProject.Contains(x.Project.Name));
                    Console.WriteLine($"After first condition: {tempQuery.Count()}");

                    // Apply the second condition
                    tempQuery = tempQuery.Where(x => receiptWithFilter_VM.SelectSubProject.Contains(x.SubProject.Name));
                    Console.WriteLine($"After second condition: {tempQuery.Count()}");

                    // Apply the third condition
                    tempQuery = tempQuery.Where(x => x.Branch == null || receiptWithFilter_VM.SelectBranchId.Contains(x.Branch.Name));
                    Console.WriteLine($"After third condition: {tempQuery.Count()}");

                    // Apply the fourth condition
                    tempQuery = tempQuery.Where(x => x.Date >= receiptWithFilter_VM.SelectedMonth1 && x.Date <= receiptWithFilter_VM.SelectedMonth2);
                    Console.WriteLine($"After fourth condition: {tempQuery.Count()}");

                    // Apply the fifth condition
                    tempQuery = tempQuery.Where(x => V_PaymentTypes.Contains(x.PaymentType));
                    Console.WriteLine($"After fifth condition: {tempQuery.Count()}");

                    // Apply the sixth condition
                    tempQuery = tempQuery.Where(x => V_GrantDestinations.Contains(x.GrantDestinations));
                    Console.WriteLine($"After sixth condition: {tempQuery.Count()}");


                    //==========================================================
                    var Items = await query
                       .Where(x =>
                       receiptWithFilter_VM.SelectProject.Contains(x.Project.Name) &&
                       receiptWithFilter_VM.SelectSubProject.Contains(x.SubProject.Name) &&
                       (x.Branch == null || receiptWithFilter_VM.SelectBranchId.Contains(x.Branch.Name)) &&
                       (x.Date >= receiptWithFilter_VM.SelectedMonth1 && x.Date <= receiptWithFilter_VM.SelectedMonth2) &&
                       V_PaymentTypes.Contains((PaymentTypes)x.PaymentType) && V_GrantDestinations.Contains((GrantDest)x.GrantDestinations))

                        .Select(a => new GetReceiptDto
                        {
                            Id = a.Id,
                            AccountNumber = a.AccountNumber,
                            Bank = a.Bank,
                            BranchName = a.Branch.Name ?? "",
                            CheckDate = a.CheckDate,
                            Code = a.Code,
                            Date = a.Date,
                            ForPurpose = a.ForPurpose,
                            Mobile = a.Mobile,
                            Number = a.Number,
                            ProjectName = a.Project.Name,
                            ReceivedByName = a.ReceivedBy,
                            TotalAmount = a.TotalAmount,
                            SubProjectName = a.SubProject.Name,
                            CheckNumber = a.CheckNumber,
                            ReceivedBy = a.ReceivedBy,
                            GrantDestinations = a.GrantDestinations,
                            PaymentType = a.PaymentType,
                            Age = a.Age,
                            Gender = a.Gender,
                            ReceivedFrom = a.ReceivedFrom,
                            BranchId = a.BranchId,
                            ProjectId = a.ProjectId,
                            SubProjectId = a.SubProjectId
                        })
                        .ToListAsync();

                    return Items;


                }

                else
                {

                    // var receipts = await _context.Receipts
                    //.Include(p => p.Branch)
                    //.Include(p => p.SubProject)
                    //.Include(p => p.Project)
                    //.Where(x =>
                    //(x.Date >= receiptWithFilter_VM.SelectedMonth1 && x.Date <= receiptWithFilter_VM.SelectedMonth2))
                    //.ToListAsync();

                    // return receipts;


                  

                    

                    var Items = await query
                        .Where(x =>
                        (x.Date >= receiptWithFilter_VM.SelectedMonth1 && x.Date <= receiptWithFilter_VM.SelectedMonth2))
                        .Select(a => new GetReceiptDto
                        {
                            Id = a.Id,
                            AccountNumber = a.AccountNumber,
                            Bank = a.Bank,
                            BranchName = a.Branch.Name??"",
                            CheckDate = a.CheckDate,
                            Code = a.Code,
                            Date = a.Date,
                            ForPurpose = a.ForPurpose,
                            Mobile = a.Mobile,
                            Number = a.Number,
                            ProjectName = a.Project.Name,
                            ReceivedByName = a.ReceivedBy,
                            TotalAmount = a.TotalAmount,
                            SubProjectName = a.SubProject.Name,
                            CheckNumber = a.CheckNumber,
                            ReceivedBy = a.ReceivedBy,
                            GrantDestinations = a.GrantDestinations,
                            PaymentType = a.PaymentType,
                            Age = a.Age,
                            Gender = a.Gender,
                            ReceivedFrom = a.ReceivedFrom,
                            BranchId = a.BranchId,
                            ProjectId = a.ProjectId,
                            SubProjectId = a.SubProjectId
                        })
                        .ToListAsync();

                    return Items;

                }

            }
        }

        public async Task<Receipt> GetReceiptRdclById(string code)
        {
            var Receipt = await _context.Receipts.Include(p => p.Branch).Include(p => p.SubProject).Where(x => x.Code == code).FirstOrDefaultAsync();


            return Receipt;
        }

        public async Task<IEnumerable<GetReceiptDto>> GetAllReceiptAsyncV2(Expression<Func<Receipt, bool>> criteria, int? PageSize, int? PageNumber, string? search,
           Expression<Func<Receipt, object>> orderBy = null, string orderByDirection = OrderBy.Decending,bool NoPagination=false)
        {
            IQueryable<Receipt> query = _context.Set<Receipt>().AsNoTracking();

            if (criteria != null)
            {
                query = query.Where(criteria);
            }



            if (PageNumber.HasValue && PageSize.HasValue && NoPagination==false)
                query = query.OrderByDescending(o => o.Number).Skip((PageNumber.Value - 1) * PageSize.Value).Take(PageSize.Value);

          

            var Items = await query.OrderByDescending(o => o.Number).Select(a => new GetReceiptDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                Bank = a.Bank,
                BranchName = a.Branch.Name,
                CheckDate = a.CheckDate,
                Code = a.Code,
                Date = a.Date,
                ForPurpose = a.ForPurpose,
                Mobile = a.Mobile,
                Number = a.Number,
                ProjectName = a.Project.Name,
                ReceivedByName = a.ReceivedBy,
                TotalAmount = a.TotalAmount,
                SubProjectName = a.SubProject.Name,
                CheckNumber = a.CheckNumber,
                ReceivedBy = a.ReceivedBy,
                GrantDestinations = a.GrantDestinations,
                PaymentType = a.PaymentType,
                Age = a.Age,
                Gender = a.Gender,
                ReceivedFrom = a.ReceivedFrom,
                BranchId = a.BranchId,
                ProjectId = a.ProjectId,
                SubProjectId = a.SubProjectId
            })
                .ToListAsync();


            return Items;


        }

       
    }
}
