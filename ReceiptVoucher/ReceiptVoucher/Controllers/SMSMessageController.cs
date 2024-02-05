using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Interfaces;
using ReceiptVoucher.Server.Components;
using System.Net;
using System.Web;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSMessageController : ControllerBase
    {
        private readonly ISMSMessage _sMSMessage;
        public SMSMessageController(ISMSMessage sMSMessage)
        {
            _sMSMessage = sMSMessage;
        }


        [HttpPost]
        public  IActionResult SendSMSMessage(Receipt receipt)
        {
            if (!ModelState.IsValid || receipt.Mobile==null)
                return BadRequest();

        

            String sendingResult;
            String username = "966535155222";
            String apiKey = "5a3eca644e8b0ba493e7d58ff6064fee09e66492";
            String numbers = receipt.Mobile; // in a comma seperated list
            String message = HttpUtility.UrlEncode("xxxxxxxxxxxxxx");
            String sender = HttpUtility.UrlEncode("xxxxxxxxxxxxxxx");

            sendingResult = _sMSMessage.send(apiKey, username, numbers, message, sender);


            return Ok(sendingResult);
        }



    }
}
