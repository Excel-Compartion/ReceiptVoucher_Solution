using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Interfaces;
using System.Net.NetworkInformation;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchesController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchRepository _branchRepository;
       
    

        public BranchesController(IUnitOfWork unitOfWork,IBranchRepository branchRepository)
        {
            _unitOfWork = unitOfWork;

            _branchRepository = branchRepository;

           
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


        [HttpPut]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(Branch branch /*, int id*/)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Edit Entity in Database using Service Or UnitOfWork : var entity = _service.Edit(model)

            Branch? editedBranch = _unitOfWork.Branches.Update(branch);

            // check if entity null , means that BadRequest , this happen during edting
            if (editedBranch is null)
                return BadRequest();

            _unitOfWork.Complete();
            return Ok(editedBranch);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {

            bool isDeleted = await _branchRepository.DeleteBranchAsync(id);

            return isDeleted ? Ok() : BadRequest("Bad Request");

        }
    }
}
