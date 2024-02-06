using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Interfaces
{
    public interface ISMSMessage
    {
        String send(String apiKey, String username, String numbers, String message, String sender);
    }
}
