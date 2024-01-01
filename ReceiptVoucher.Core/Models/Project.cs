namespace ReceiptVoucher.Core.Models;

public  class Project
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    //-------- Navigation Properties ---
    public ICollection<SubProject> SubProjects { get; set; } = new List<SubProject>();
}
