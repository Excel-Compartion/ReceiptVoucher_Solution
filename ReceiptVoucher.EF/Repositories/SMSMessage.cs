using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.EF.Repositories
{
    public class SMSMessage : ISMSMessage
    {
        public String send(String apiKey, String username, String numbers, String message, String sender)
        {

                String result;

                String sendUrl = "https://www.mora-sa.com/api/v1/sendsms?api_key=" + apiKey + "&username=" + username + "&message=" + message + "&sender=" + sender + "&numbers=" + numbers + "&response=text";

                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(sendUrl);

                ((HttpWebRequest)objRequest).ServicePoint.BindIPEndPointDelegate += (servicePoint, remoteEndPoint, retryCount) => {
                    Console.WriteLine("BindIPEndpoint called");
                    return new IPEndPoint(IPAddress.Parse("212.93.191.190"), 443);
                };

                objRequest.Method = "GET";
                objRequest.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }


                return  result;
            
        }
    }
}
