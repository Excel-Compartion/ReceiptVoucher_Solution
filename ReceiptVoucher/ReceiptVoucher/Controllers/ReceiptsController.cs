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
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;
using static MudBlazor.CategoryTypes;
using Pagination = ReceiptVoucher.Core.Models.Pagination;


namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReceiptsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IMapper mapper;


        private readonly UserManager<ApplicationUser> _userManager;

        IWebHostEnvironment webHostEnvironment;

        public ReceiptsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IReceiptRepository receiptRepository, IWebHostEnvironment WebHostEnvi, IMapper _mapper)
        {
            _unitOfWork = unitOfWork;
            _receiptRepository = receiptRepository;

            _userManager = userManager; 

            webHostEnvironment = WebHostEnvi;

            mapper = _mapper;
        }
        public DataTable ToDataTable<T>(List<T> list)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            foreach (T instance in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in properties)
                {
                    row[info.Name] = info.GetValue(instance) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public DataTable ToDataTableOneRecord<T>(T instance)
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




        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Receipt> recepts = await _unitOfWork.Receipts.GetAllReceiptAsync();

            return Ok(recepts);
        }


        [HttpPost("GetAllReceiptsWithGetDto")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetReceiptDto>>>> GetAllPurchases([FromBody] Pagination pagination)
        {
            try
            {
                //                IEnumerable<Receipt> items = await _unitOfWork.Receipts.FindAllAsync(search: pagination?.Search, criteria: null, PageSize: pagination.PageSize, PageNumber: pagination.PageNumber, includes: ["Branch", "Project", "SubProject"]);
                //                var receipts = (await _unitOfWork.Receipts.GetAllAsync()).Select(a => new GetReceiptDto 
                //                { 
                //Id=a.Id,
                //BranchName = a.Branch.Name,

                //                }).ToList();


                //                IEnumerable<GetReceiptDto> Items = mapper.Map<List<GetReceiptDto>>(items);

                IEnumerable<GetReceiptDto> items = await _unitOfWork.Receipts.GetAllReceiptAsyncV2(search: pagination?.Search, criteria: null, PageSize: pagination.PageSize, PageNumber: pagination.PageNumber);

                return Ok(new BaseResponse<IEnumerable<GetReceiptDto>>(items, "تم جلب العناصر بنجاح", null, true));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<GetReceiptDto>(null, "حدث خطأ", errors: [ex.ToString()], false));
            }
        }



        [HttpPost]
        [Route("AddReceipt")]
        public async Task<IActionResult> AddReceipt([FromBody] Receipt receipt)
        {
            // Generate a unique code
            //receipt.Code = Guid.NewGuid().ToString();

            string code = Guid.NewGuid().ToString("N").Substring(0, 8);

            var ReceiptIsEx = await _unitOfWork.Receipts.FindAsync(x => x.Code == code);

            while (ReceiptIsEx != null)
            {
                code = Guid.NewGuid().ToString("N").Substring(0, 8);
                ReceiptIsEx = await _unitOfWork.Receipts.FindAsync(x => x.Code == code);
            }

            receipt.Code = code;

            // Get the last receipt and increment the number
            var lastReceipt = await _receiptRepository.GetLastAsync();
           int Number = (lastReceipt?.Number ?? 0) + 1;

            var ReceiptIsExByNumber = await _unitOfWork.Receipts.FindAsync(x => x.Number == Number);

            while (ReceiptIsExByNumber != null)
            {
                 lastReceipt = await _receiptRepository.GetLastAsync();
                 Number = (lastReceipt?.Number ?? 0) + 1;
                ReceiptIsExByNumber = await _unitOfWork.Receipts.FindAsync(x => x.Number == Number);
            }

            receipt.Number = Number;
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

           

         var receipts =  await _receiptRepository.GetFilteredData(filterData);


            return Ok(receipts);
        }


        [HttpPost("GetDonorCorrespondenceWithFilteredData")]
        public async Task<IActionResult> GetDonorCorrespondenceWithFilteredData(FilterData filterData)
        {


            if (!ModelState.IsValid)
                return BadRequest();

            var receipts = await _receiptRepository.GetFilteredData(filterData);


            List<GrantDestination_VM> DonorCorrespondences = mapper.Map<List<GrantDestination_VM>>(receipts.GroupBy(x => x.Mobile)
               .Select(group => group.First()));

            return Ok(DonorCorrespondences);
        }


        [HttpPost("GetReportWithFilteredData")]
        public async Task<IActionResult> GetReportWithFilteredData(FilterData filterData)
        {


            if (!ModelState.IsValid)
            {
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest("خطاء في البيانات المدخلة");
            }



            var receipts = await _receiptRepository.GetFilteredData(filterData);



            if (receipts == null)
            {
                return BadRequest($"حدث خطاء اتناء جلب البيانات ");
            }

            List<ReceiptWithRelatedDataDto> receiptWithRelatedDataDto = mapper.Map<List<ReceiptWithRelatedDataDto>>(receipts);

            ReceiptsInformation receiptsInformation = new ReceiptsInformation()
            {
                TotalAmount=  $"اجمالي مبالغ التبرعات : {receiptWithRelatedDataDto.Select(x => x.TotalAmount).Sum()} ر.س",
                ReceiptsCount= $"اجمالي عدد السندات : {receiptWithRelatedDataDto.Count()}"
            };

            string path = webHostEnvironment.WebRootPath + @"\_Reports\Report1.rdlc";

            LocalReport localReport = new LocalReport(path);

            DataTable dt = ToDataTable(receiptWithRelatedDataDto);

            DataTable info = ToDataTableOneRecord(receiptsInformation);

            localReport.AddDataSource("DataSet1", dt);
            localReport.AddDataSource("ReceiptsInformationDataSet", info);




            var report = localReport.Execute(RenderType.Excel);


            return File(report.MainStream, "application/msexcel", "myReport.xls");

        }


        [HttpPost("GetDonorsCorrespondencesReportWithFilteredData")]
        public async Task<IActionResult> GetDonorsCorrespondencesReportWithFilteredData(FilterData filterData)
        {


            if (!ModelState.IsValid)
            {
                var ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest("خطاء في البيانات المدخلة");
            }


            var receipts = await _receiptRepository.GetFilteredData(filterData);

            if (receipts == null)
            {
                return BadRequest($"حدث خطاء اتناء جلب البيانات ");
            }

            List<GrantDestination_VM> grantDestination = mapper.Map<List<GrantDestination_VM>>(receipts.GroupBy(x => x.Mobile)
               .Select(group => group.First()));


            string path = webHostEnvironment.WebRootPath + @"\_Reports\GrantDestinationsReport\GrantDestinationsReport.rdlc";

            LocalReport localReport = new LocalReport(path);

            DataTable grDest = ToDataTable(grantDestination);

            localReport.AddDataSource("GrantDestinationDataSet", grDest);




            var report = localReport.Execute(RenderType.Excel);


            return File(report.MainStream, "application/msexcel", "GrantDestination.xls");

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
