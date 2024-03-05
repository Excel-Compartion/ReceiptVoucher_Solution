using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Identity;
using ReceiptVoucher.Core.Interfaces;
using SU.StudentServices.Data.Helpers;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RPController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRepository _receiptRepository;


        private readonly UserManager<ApplicationUser> _userManager;

        IWebHostEnvironment webHostEnvironment;

        public RPController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IReceiptRepository receiptRepository, IWebHostEnvironment WebHostEnvi)
        {
            _unitOfWork = unitOfWork;
            _receiptRepository = receiptRepository;

            _userManager = userManager;

            webHostEnvironment = WebHostEnvi;
        }


        public DataTable ToDataTable<T>(T instance)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            DataRow row = dt.NewRow();
            foreach (PropertyInfo info in properties)
            {
                row[info.Name] = info.GetValue(instance) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
            return dt;
        }


        [HttpGet("{code}")]
        public async Task<IActionResult> GetReceiptRdcl(string code)
        {
            var Receipt = await _receiptRepository.GetReceiptRdclById(code);

            var user = await _userManager.Users.Where(a => a.Id == Receipt.ReceivedBy).FirstOrDefaultAsync();

            ReceiptRdclViewModel receiptRdclViewModel = new ReceiptRdclViewModel();

            receiptRdclViewModel.Id = Receipt.Id;
            receiptRdclViewModel.ReceivedFrom = Receipt.ReceivedFrom;



            receiptRdclViewModel.ReceivedBy = user.FirstName + " " + user.LastName;



            receiptRdclViewModel.TotalAmount = Receipt.TotalAmount + "";
            receiptRdclViewModel.Branch = Receipt.Branch.Name + "";
            receiptRdclViewModel.SubProject = Receipt.SubProject.Name + "";
            receiptRdclViewModel.ForPurpose = Receipt.ForPurpose;
            receiptRdclViewModel.Date = Receipt.Date.ToString("yyyy-MM-dd") + "";
            receiptRdclViewModel.PaymentType = Receipt.PaymentType.GetDisplayName();
            receiptRdclViewModel.CheckNumber = Receipt.CheckNumber + "";
            receiptRdclViewModel.Number = Receipt.Number + "";


            //تحويل تاريخ الشيك الى تاريخ هجري
            DateOnly? gregDate2 = Receipt.CheckDate;
            CultureInfo ci2 = new CultureInfo("ar-SA");
            var checkDate = gregDate2?.ToString("dd/MM/yyyy", ci2);


            receiptRdclViewModel.CheckDate = checkDate;


            receiptRdclViewModel.AccountNumber = Receipt.AccountNumber + "";
            receiptRdclViewModel.Bank = Receipt.Bank;

            receiptRdclViewModel.GrantDestinations = Receipt.GrantDestinations.GetDisplayName();

            if (Receipt.GrantDestinations.GetDisplayName() == GrantDest.Individual.GetDisplayName())
            {
                receiptRdclViewModel.Gender = Receipt.Gender.GetDisplayName();
                receiptRdclViewModel.Age = Receipt.Age.GetDisplayName();
            }

            receiptRdclViewModel.Mobile = Receipt.Mobile + "";


            NumberToWord numberToWord = new(Convert.ToDecimal(Receipt.TotalAmount), new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));

            string Text = numberToWord.ConvertToArabic();
            receiptRdclViewModel.TotalAmountWord = Text.Replace(".", "");

            //تحويل تاريخ السند الى تاريخ هجري
            DateOnly gregDate = Receipt.Date;
            CultureInfo ci = new CultureInfo("ar-SA");
            var D = gregDate.ToString("dd/MM/yyyy", ci);


            receiptRdclViewModel.DateArabic = D;


            string path = webHostEnvironment.WebRootPath + @"\_Reports\ReceiptRdcl\ReceiptRdcl.rdlc";

            LocalReport localReport = new LocalReport(path);

            DataTable dt = ToDataTable(receiptRdclViewModel);
            localReport.AddDataSource("ReceiptDataSet", dt);




            var report = localReport.Execute(RenderType.Pdf,1);


            return File(report.MainStream, "application/pdf");
        }




        [HttpGet("Report/{code}")]
        public async Task<IActionResult> GetReceiptReportRdcl(string code)
        {
            var Receipt = await _receiptRepository.GetReceiptRdclById(code);

            var user = await _userManager.Users.Where(a => a.Id == Receipt.ReceivedBy).FirstOrDefaultAsync();

            ReceiptRdclViewModel receiptRdclViewModel = new ReceiptRdclViewModel();


      
            receiptRdclViewModel.Id = Receipt.Id;
            receiptRdclViewModel.ReceivedFrom = Receipt.ReceivedFrom;



            receiptRdclViewModel.ReceivedBy = user.FirstName + " " + user.LastName;



            receiptRdclViewModel.TotalAmount = Receipt.TotalAmount + "";
            receiptRdclViewModel.Branch = Receipt.Branch.Name + "";
            receiptRdclViewModel.SubProject = Receipt.SubProject.Name + "";
            receiptRdclViewModel.ForPurpose = Receipt.ForPurpose;
            receiptRdclViewModel.Date = Receipt.Date.ToString("yyyy-MM-dd") + "";
            receiptRdclViewModel.PaymentType = Receipt.PaymentType.GetDisplayName();
            receiptRdclViewModel.CheckNumber = Receipt.CheckNumber + "";
            receiptRdclViewModel.Number = Receipt.Number + "";


            //تحويل تاريخ الشيك الى تاريخ هجري
            DateOnly? gregDate2 = Receipt.CheckDate;
            CultureInfo ci2 = new CultureInfo("ar-SA");
            var checkDate = gregDate2?.ToString("dd/MM/yyyy", ci2);


            receiptRdclViewModel.CheckDate = checkDate;


            receiptRdclViewModel.AccountNumber = Receipt.AccountNumber + "";
            receiptRdclViewModel.Bank = Receipt.Bank;

            receiptRdclViewModel.GrantDestinations = Receipt.GrantDestinations.GetDisplayName();

            if (Receipt.GrantDestinations.GetDisplayName() == GrantDest.Individual.GetDisplayName())
            {
                receiptRdclViewModel.Gender = Receipt.Gender.GetDisplayName();
                receiptRdclViewModel.Age = Receipt.Age.GetDisplayName();
            }

            receiptRdclViewModel.Mobile = Receipt.Mobile + "";


            NumberToWord numberToWord = new(Convert.ToDecimal(Receipt.TotalAmount), new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));

            string Text = numberToWord.ConvertToArabic();
            receiptRdclViewModel.TotalAmountWord = Text.Replace(".", "");

            //تحويل تاريخ السند الى تاريخ هجري
            DateOnly gregDate = Receipt.Date;
            CultureInfo ci = new CultureInfo("ar-SA");
            var D = gregDate.ToString("dd/MM/yyyy", ci);


            receiptRdclViewModel.DateArabic = D;


            string path = webHostEnvironment.WebRootPath + @"\_Reports\Report1.rdlc";

            LocalReport localReport = new LocalReport(path);

            DataTable dt = ToDataTable(receiptRdclViewModel);
            localReport.AddDataSource("DataSet1", dt);




            var report = localReport.Execute(RenderType.Excel);


            return File(report.MainStream, "application/msexcel", "myReport.xls");
        }



    }
}
