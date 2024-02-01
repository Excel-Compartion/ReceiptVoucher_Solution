using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Consts;
using ReceiptVoucher.Core.Interfaces;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        IWebHostEnvironment webHostEnvironment;

        public ProjectsController(IUnitOfWork unitOfWork, IProjectRepository projectRepository, IWebHostEnvironment WebHostEnvi)
        {
            _unitOfWork = unitOfWork;

            _projectRepository = projectRepository;

            webHostEnvironment = WebHostEnvi;
        }


        [HttpGet("GetReport1")]
        public async Task<IActionResult> GetReport1()
        {
            string path = webHostEnvironment.WebRootPath + @"\_Reports\Report1.rdlc";

            LocalReport localReport = new LocalReport(path);

            var data = await _unitOfWork.Projects.GetAllAsync();

            localReport.AddDataSource("DataSet1", data);

            var report = localReport.Execute(RenderType.Pdf, 1);


            return File(report.MainStream, "application/pdf");
        }


        //[HttpGet("GetReport1")]
        //public async Task<IActionResult> GetReport1()
        //{
        //    string path = webHostEnvironment.WebRootPath + @"\_Reports\Report1.rdlc";

        //    LocalReport localReport = new LocalReport(path);

        //    var data = await _unitOfWork.Projects.GetAllAsync();

        //    localReport.AddDataSource("DataSet1", data);

        //    var report = localReport.Execute(RenderType.Html, 1);

        //    var reportString = System.Text.Encoding.UTF8.GetString(report.MainStream);

        //    return Content(reportString, "text/html");
        //}


        //[Authorize(Roles = RolesNames.Admin)]
        [Authorize]
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.Projects.GetAllAsync());
        }

        [HttpPost("AddOneAsync")]
        public async Task<IActionResult> AddOne(Project project)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // let UnitOfWork  do creating and saving to database

            await _unitOfWork.Projects.AddOneAsync(project);

            return Ok("project Created Succesfully.");
        }


        [HttpPut]
        public async Task<IActionResult> Update(Project project)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // استخدم الدالة UpdateProjectAsync لتحديث المشروع
            bool isUpdated = await _projectRepository.UpdateProjectAsync(project);

            // إذا حدث خطأ أثناء التحديث، قم بإرجاع BadRequest
            if (!isUpdated)
                return BadRequest();

            // إذا تم التحديث بنجاح، قم بإرجاع Ok
            return Ok();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await _projectRepository.DeleteProjectAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}

