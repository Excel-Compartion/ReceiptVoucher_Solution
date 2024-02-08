using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Interfaces;
using ReceiptVoucher.Server.Components;
using System.Net;
using System.Text.RegularExpressions;
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
            if (!ModelState.IsValid )
                return BadRequest("بيانات السند غير صالحه");



            if(receipt.Mobile == null || receipt.Mobile == "")
            {
                return BadRequest("حقل الرقم غير مدخل");
            }

            string receiptMobile = receipt.Mobile;

            // إزالة المسافات من الرقم
            string mobileWithoutSpaces = receiptMobile.Replace(" ", "");



            // التحقق مما إذا كانت القيمة المتبقية هي عبارة عن أرقام فقط
            bool isDigitsOnly = mobileWithoutSpaces.All(char.IsDigit);

            if (isDigitsOnly == false && !mobileWithoutSpaces.Contains("+"))
            {
                return BadRequest("الرقم غير صالح");
            }


            try
            {
                String sendingResult;
                String username = "966535155222";
                String apiKey = "5a3eca644e8b0ba493e7d58ff6064fee09e66492";
                String numbers = receipt.Mobile; // in a comma seperated list
                String message = HttpUtility.UrlEncode("xxxxxxxxxxxxxx");
                String sender = HttpUtility.UrlEncode("966535155222");

                sendingResult = _sMSMessage.send(apiKey, username, numbers, message, sender);

                if (sendingResult != "106")
                {
                    return BadRequest("فشل الارسال");
                }


                return Ok(sendingResult);
            }

            catch { BadRequest("Error"); }


            return Ok();
        }



    }
}
