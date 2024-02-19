using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Interfaces;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
using Humanizer;
using System.Globalization;
using SU.StudentServices.Data.Helpers;
using Microsoft.AspNetCore.Identity;
using ReceiptVoucher.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using ReceiptVoucher.EF.Repositories;
using ReceiptVoucher.Core.Models.ResponseModels;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceiptsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRepository _receiptRepository;


        private readonly UserManager<ApplicationUser> _userManager;

        IWebHostEnvironment webHostEnvironment;

        public ReceiptsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IReceiptRepository receiptRepository, IWebHostEnvironment WebHostEnvi)
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

        //[AllowAnonymous]
  
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetReceiptRdcl(int id)
        //{
        //    var Receipt = await _receiptRepository.GetReceiptRdclById(id);

        //    var user = await _userManager.Users.Where(a => a.Id == Receipt.ReceivedBy).FirstOrDefaultAsync();

        //    ReceiptRdclViewModel receiptRdclViewModel = new ReceiptRdclViewModel();

        //    receiptRdclViewModel.Id = Receipt.Id;
        //    receiptRdclViewModel.ReceivedFrom = Receipt.ReceivedFrom;



        //    receiptRdclViewModel.ReceivedBy = user.FirstName + " " + user.LastName;

           

        //    receiptRdclViewModel.TotalAmount = Receipt.TotalAmount + "";
        //    receiptRdclViewModel.Branch = Receipt.Branch.Name + "";
        //    receiptRdclViewModel.SubProject = Receipt.SubProject.Name + "";
        //    receiptRdclViewModel.ForPurpose = Receipt.ForPurpose;
        //    receiptRdclViewModel.Date = Receipt.Date.ToString("yyyy-MM-dd") + "";
        //    receiptRdclViewModel.PaymentType = Receipt.PaymentType.GetDisplayName();
        //    receiptRdclViewModel.CheckNumber = Receipt.CheckNumber + "";

          
        //        //تحويل تاريخ الشيك الى تاريخ هجري
        //        DateOnly? gregDate2 = Receipt.CheckDate;
        //        CultureInfo ci2 = new CultureInfo("ar-SA");
        //        var checkDate = gregDate2?.ToString("dd/MM/yyyy", ci2);


        //        receiptRdclViewModel.CheckDate = checkDate;
          

        //    receiptRdclViewModel.AccountNumber = Receipt.AccountNumber + "";
        //    receiptRdclViewModel.Bank = Receipt.Bank;

        //    receiptRdclViewModel.GrantDestinations = Receipt.GrantDestinations.GetDisplayName();

        //    if (Receipt.GrantDestinations.GetDisplayName() == GrantDest.Individual.GetDisplayName())
        //    {
        //        receiptRdclViewModel.Gender = Receipt.Gender.GetDisplayName();
        //        receiptRdclViewModel.Age = Receipt.Age.GetDisplayName();
        //    }

        //    receiptRdclViewModel.Mobile = Receipt.Mobile + "";


        //    NumberToWord numberToWord=new (Convert.ToDecimal(Receipt.TotalAmount) ,new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));

        //    string Text= numberToWord.ConvertToArabic();
        //    receiptRdclViewModel.TotalAmountWord = Text.Replace(".", "");

        //    //تحويل تاريخ السند الى تاريخ هجري
        //    DateOnly gregDate = Receipt.Date; 
        //    CultureInfo ci = new CultureInfo("ar-SA");
        //  var D=   gregDate.ToString("dd/MM/yyyy", ci);


        //    receiptRdclViewModel.DateArabic = D;


        //    string path = webHostEnvironment.WebRootPath + @"\_Reports\ReceiptRdcl\ReceiptRdcl.rdlc";

        //    LocalReport localReport = new LocalReport(path);

        //    DataTable dt = ToDataTable(receiptRdclViewModel);
        //    localReport.AddDataSource("ReceiptDataSet", dt);


        //    var report = localReport.Execute(RenderType.Pdf, 1);


        //    return File(report.MainStream, "application/pdf");
        //}





        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _receiptRepository.GetAllReceiptAsync());
        }




        [HttpPost]
        [Route("AddReceipt")]
        public async Task<IActionResult> AddReceipt([FromBody] Receipt receipt)
        {
            // Generate a unique code
            //receipt.Code = Guid.NewGuid().ToString();

            receipt.Code = Guid.NewGuid().ToString("N").Substring(0, 12);


            // Get the last receipt and increment the number
            var lastReceipt = await _receiptRepository.GetLastAsync();
            receipt.Number = (lastReceipt?.Number ?? 0) + 1;

            receipt.Date = DateOnly.FromDateTime(DateTime.Now);
            if (!ModelState.IsValid)
            {
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(new BaseResponse<Receipt>(receipt, "خطاء في البيانات المدخله", errors: ModelErrors, success: false));
            }

            // let UnitOfWork  do creating and saving to database
            Receipt response = await _unitOfWork.Receipts.AddOneAsync(receipt);

            if (receipt.Id == 0)
            {
                return BadRequest(new BaseResponse<Receipt>(receipt, "حدث خطاء اثناء الاضافة", errors: null, success: false));
            }

            return Ok(new BaseResponse<Receipt>(receipt, "تمت الاضافة بنجاح", errors: null, success: true));
        }










        [HttpPost("GetFilteredData")]
        public async Task<IActionResult> GetFilteredData(FilterData filterData)
        {

           
            if (!ModelState.IsValid)
                return BadRequest();

           

         var receipt =  await _receiptRepository.GetFilteredData(filterData);


            return Ok(receipt);
        }


        [HttpPut]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(Receipt receipt )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Edit Entity in Database using Service Or UnitOfWork : var entity = _service.Edit(model)

            Receipt? editedReceipt = _unitOfWork.Receipts.Update(receipt);

            // check if entity null , means that BadRequest , this happen during edting
            if (editedReceipt is null)
                return BadRequest();

            _unitOfWork.Complete(); 
            return Ok(editedReceipt);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await _unitOfWork.Receipts.DeleteAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}
