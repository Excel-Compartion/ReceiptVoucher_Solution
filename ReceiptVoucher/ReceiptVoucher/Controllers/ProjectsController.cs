using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Interfaces;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProjectsController(IUnitOfWork unitOfWork, IProjectRepository projectRepository)
        {
            _unitOfWork = unitOfWork;

            _projectRepository = projectRepository;
        }

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
            bool isDeleted = await _unitOfWork.Projects.DeleteAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}

