using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReceiptVoucher.Core.Enums;
using ReceiptVoucher.Core.Interfaces;
using ReceiptVoucher.Core.Models.ResponseModels;
using ReceiptVoucher.Server.Components;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                String number = mobileWithoutSpaces; // in a comma seperated list
                String message = HttpUtility.UrlEncode($"نشكر لكم دعمكم مشاريع جمعية اكرام للتفاصيل:\n https://receiptvoucher.dlelkrs.com/ReceiptPrints/{receipt.Id}");
                String sender = HttpUtility.UrlEncode("MAKARIM");

                sendingResult = _sMSMessage.send(apiKey, username, number, message, sender);

                string Message="خطاء";

                if (sendingResult != "100")
                {
                    
                    switch (sendingResult)
                    {
                        case "105":
                            Message = "الرصيد لايكفي";
                            break;

                        case "106":
                            Message = "اسم المرسل غير متاح";
                            break;

                        case "107":
                            Message = "اسم المرسل محجوب";
                            break;

                        case "108":
                            Message = "لايوحد ارقام صالحة للارسال";
                            break;

                        case "112":
                            Message = "الرسالة تحتوي على كلمة محظورة";
                            break;

                        case "114":
                            Message = "الحساب موقوف";
                            break;

                        case "115":
                            Message = "جوال غير مفعل";
                            break;
                        case "116":
                            Message = "بريد الكتروني غير مفعل";
                            break;

                        case "117":
                            Message = "الرسالة فارغة ولايمكن ارسالها";
                            break;

                        case "118":
                            Message = "اسم المرسل فارغ";
                            break;

                        case "119":
                            Message = "لم يتم وضع رقم مستقبل";
                            break;
                        case "121":
                            Message = "حظر على الرسائل الاعلانية (-AD)";
                            break;

                    }

                    return BadRequest(new BaseResponse<string>(null, Message, errors: null, success: false)); 
                }


                return Ok(new BaseResponse<string>(sendingResult, "تم ارسال الرسالة بنجاح", errors: null, success:true));
            }

            catch{return BadRequest(new BaseResponse<string>(null, "حدث خطاء ", errors: null, success: false)); }


           
        }



    }
}
