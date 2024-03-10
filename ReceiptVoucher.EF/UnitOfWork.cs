
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Identity;

namespace ReceiptVoucher.EF
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ReceiptVoucherDbContext _context;

        private readonly IMapper mapper;
        //public IBaseRepository<Branch> Branches { get; private set; }

        public IBranchRepository Branches { get; private set; }

        public IBaseRepository<Project> Projects { get; private set; }

        public IBaseRepository<ApplicationUser> Users { get; private set; }

        public IBaseRepository<CompanyInfo> CompanyInfo { get; private set; }

        //public IBaseRepository<Receipt> Receipts { get; private set; }
        public IReceiptRepository Receipts { get; private set; }    // new 

        public IBaseRepository<SubProject> SubProjects { get; private set; }

        public UnitOfWork(ReceiptVoucherDbContext context, IMapper _mapper)
        {
            _context = context;
            mapper = _mapper;
            Branches = new BranchRepository(context);
            Projects = new BaseRepository<Project>(context);
            Users = new BaseRepository<ApplicationUser>(context);
            CompanyInfo = new BaseRepository<CompanyInfo>(context);
            Receipts = new ReceiptRepository(context, mapper);  // new 
            SubProjects = new BaseRepository<SubProject>(context);

        }

        public int Complete()
        {
            return  _context.SaveChanges();
        }


      
        public void Detach<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
