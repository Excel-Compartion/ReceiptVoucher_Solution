using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Entities;
using ReceiptVoucher.Core.Interfaces;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRepository _receiptRepository;
        public ReceiptsController(IUnitOfWork unitOfWork, IReceiptRepository receiptRepository)
        {
            _unitOfWork = unitOfWork;
            _receiptRepository = receiptRepository;
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
