using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyInfosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyInfosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.CompanyInfo.GetAllAsync());
        }

        [HttpPost("AddOneAsync")]
        public async Task<IActionResult> AddOne(CompanyInfo companyInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // let UnitOfWork  do creating and saving to database

            await _unitOfWork.CompanyInfo.AddOneAsync(companyInfo);

            return Ok("companyInfo Created Succesfully.");
        }


        [HttpPut]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(CompanyInfo companyInfo /*, int id*/)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Edit Entity in Database using Service Or UnitOfWork : var entity = _service.Edit(model)

            CompanyInfo? editedSupProject = _unitOfWork.CompanyInfo.Update(companyInfo);

            // check if entity null , means that BadRequest , this happen during edting
            if (editedSupProject is null)
                return BadRequest();

            _unitOfWork.Complete();
            return Ok(editedSupProject);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await _unitOfWork.CompanyInfo.DeleteAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}
