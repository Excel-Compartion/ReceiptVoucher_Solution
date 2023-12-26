
namespace ReceiptVoucher.EF
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ReceiptVoucherDbContext _context;


        public IBaseRepository<Branch> Branches { get; private set; }

        public IBaseRepository<Project> Projects { get; private set; }

        public IBaseRepository<CompanyInfo> CompanyInfo { get; private set; }

        public IBaseRepository<Receipt> Receipts { get; private set; }

        public IBaseRepository<SubProject> SubProjects { get; private set; }

        public UnitOfWork(ReceiptVoucherDbContext context )
        {
            _context = context;

            Branches = new BaseRepository<Branch>(context);
            Projects = new BaseRepository<Project>(context);
            CompanyInfo = new BaseRepository<CompanyInfo>(context);
            Receipts = new BaseRepository<Receipt>(context);
            SubProjects = new BaseRepository<SubProject>(context);

        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
