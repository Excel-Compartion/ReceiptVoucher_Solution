using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(Project project /*, int id*/)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Edit Entity in Database using Service Or UnitOfWork : var entity = _service.Edit(model)

            Project? editedProject = _unitOfWork.Projects.Update(project);

            // check if entity null , means that BadRequest , this happen during edting
            if (editedProject is null)
                return BadRequest();

            _unitOfWork.Complete();
            return Ok(editedProject);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await _unitOfWork.Projects.DeleteAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}

