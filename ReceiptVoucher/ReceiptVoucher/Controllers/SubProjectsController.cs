using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Interfaces;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubProjectRepository _subProjectRepository;
     

        public SubProjectsController(IUnitOfWork unitOfWork, ISubProjectRepository subProjectRepository, ReceiptVoucherDbContext context)
        {
            _unitOfWork = unitOfWork;

            _subProjectRepository = subProjectRepository;
           
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.SubProjects.GetAllAsync());
        }

        // this include the navg_prop

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _subProjectRepository.GetAllSubProjectAsync());
        }


        [HttpPost("AddOneAsync")]
        public async Task<IActionResult> AddOne(SubProject subProject)
        {
            subProject.CreatedDate = DateTime.Now;
            
           
            if (!ModelState.IsValid)
                return BadRequest();

            // let UnitOfWork  do creating and saving to database

            await _unitOfWork.SubProjects.AddOneAsync(subProject);

            return Ok("Branch Created Succesfully.");
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
