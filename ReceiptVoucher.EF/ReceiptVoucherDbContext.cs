
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Identity;

namespace ReceiptVoucher.EF;

public class ReceiptVoucherDbContext : IdentityDbContext<ApplicationUser>
{
    public ReceiptVoucherDbContext()
    {
    }

    public ReceiptVoucherDbContext(DbContextOptions<ReceiptVoucherDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<SubProject> SubProjects { get; set; }




}
