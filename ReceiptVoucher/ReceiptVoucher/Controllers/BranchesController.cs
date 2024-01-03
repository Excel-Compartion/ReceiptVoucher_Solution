using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public BranchesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.Branches.GetAllAsync());
        }

       [HttpPost("AddOneAsync")]
        public async Task<IActionResult> AddOne(Branch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // let UnitOfWork  do creating and saving to database

            await _unitOfWork.Branches.AddOneAsync(branch);

            return Ok("Branch Created Succesfully.");
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAsycn(int id)
        {
            bool isDeleted = await _unitOfWork.Branches.DeleteAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}
