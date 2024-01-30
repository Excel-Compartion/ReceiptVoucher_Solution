namespace ReceiptVoucher.Core.Entities;

public class Project
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "يرجى ادخال اسم المشروع")]
    [StringLength(250)]
    public string Name { get; set; } = null!;

    public string? Note {  get; set; }

    public bool IsActive { get; set; } = true;

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    //-------- Navigation Properties ---
    public ICollection<SubProject> SubProjects { get; set; } = new List<SubProject>();
}
