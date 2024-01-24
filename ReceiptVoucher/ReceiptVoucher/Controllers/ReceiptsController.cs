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

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRepository _receiptRepository;

        IWebHostEnvironment webHostEnvironment;

        public ReceiptsController(IUnitOfWork unitOfWork, IReceiptRepository receiptRepository, IWebHostEnvironment WebHostEnvi)
        {
            _unitOfWork = unitOfWork;
            _receiptRepository = receiptRepository;

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

        [HttpGet("GetReceiptRdcl/{id}")]
        public async Task<IActionResult> GetReceiptRdcl(int id)
        {
            var Receipt = await _receiptRepository.GetReceiptRdclById(id);

            ReceiptRdclViewModel receiptRdclViewModel = new ReceiptRdclViewModel();

            receiptRdclViewModel.Id = Receipt.Id;
            receiptRdclViewModel.ReceivedFrom = Receipt.ReceivedFrom;
            receiptRdclViewModel.ReceivedBy = Receipt.ReceivedBy;
            receiptRdclViewModel.TotalAmount = Receipt.TotalAmount + "";
            receiptRdclViewModel.Branch = Receipt.Branch.Name + "";
            receiptRdclViewModel.SubProject = Receipt.SubProject.Name + "";
            receiptRdclViewModel.ForPurpose = Receipt.ForPurpose;
            receiptRdclViewModel.Date = Receipt.Date + "";
            receiptRdclViewModel.PaymentType = Receipt.PaymentType.GetDisplayName();
            receiptRdclViewModel.CheckNumber = Receipt.CheckNumber + "";
            receiptRdclViewModel.AccountNumber = Receipt.AccountNumber + "";
            receiptRdclViewModel.Bank = Receipt.Bank;
            receiptRdclViewModel.GrantDestinations = Receipt.GrantDestinations.GetDisplayName();
            receiptRdclViewModel.Gender = Receipt.Gender.GetDisplayName();
            receiptRdclViewModel.Age = Receipt.Age.GetDisplayName();
            receiptRdclViewModel.Mobile = Receipt.Mobile + "";

            receiptRdclViewModel.TotalAmountWord = Convert.ToInt32(Receipt.TotalAmount).ToWords() +"     ريال";



            DateOnly gregDate = Receipt.Date; 
            CultureInfo ci = new CultureInfo("ar-SA");
          var D=   gregDate.ToString("dd/MM/yyyy", ci);


            receiptRdclViewModel.DateArabic = D;




          




            string path = webHostEnvironment.WebRootPath + @"\_Reports\ReceiptRdcl\ReceiptRdcl.rdlc";

            LocalReport localReport = new LocalReport(path);

            DataTable dt = ToDataTable(receiptRdclViewModel);
            localReport.AddDataSource("ReceiptDataSet", dt);


            var report = localReport.Execute(RenderType.Pdf, 1);


            return File(report.MainStream, "application/pdf");
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _receiptRepository.GetAllSubProjectAsync());
        }

        [HttpPost("AddOneAsync")]
        public async Task<IActionResult> AddOne(Receipt receipt)
        {
            
            receipt.Date=DateOnly.FromDateTime(DateTime.Now);
            if (!ModelState.IsValid)
                return BadRequest();

            // let UnitOfWork  do creating and saving to database

            await _unitOfWork.Receipts.AddOneAsync(receipt);
            

            return Ok("receipt Created Succesfully.");
        }


        [HttpPut]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(Receipt receipt /*, int id*/)
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
