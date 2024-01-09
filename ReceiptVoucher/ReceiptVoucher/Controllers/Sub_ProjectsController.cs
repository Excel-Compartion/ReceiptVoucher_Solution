using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Sub_ProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public Sub_ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.SubProjects.GetAllAsync());
        }

        [HttpPost("AddOneAsync")]
        public async Task<IActionResult> AddOne(SubProject subProject)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // let UnitOfWork  do creating and saving to database

            await _unitOfWork.SubProjects.AddOneAsync(subProject);

            return Ok("subProject Created Succesfully.");
        }


        [HttpPut]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(SubProject subProject /*, int id*/)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Edit Entity in Database using Service Or UnitOfWork : var entity = _service.Edit(model)

            SubProject? editedSupProject = _unitOfWork.SubProjects.Update(subProject);

            // check if entity null , means that BadRequest , this happen during edting
            if (editedSupProject is null)
                return BadRequest();

            _unitOfWork.Complete();
            return Ok(editedSupProject);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await _unitOfWork.SubProjects.DeleteAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}
