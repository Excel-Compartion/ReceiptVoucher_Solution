using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptVoucher.Core.Models.ViewModels.UserModels
{
    public class UserRolesViewModel
    {
        
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<RoleViewModel>? Roles { get; set; }
        
    }
}
