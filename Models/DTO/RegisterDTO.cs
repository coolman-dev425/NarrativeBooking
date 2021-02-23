using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Identity
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string StripeEmail { get; set; }
        public string DebitCardNo { get; set; }
        public int DebitExpMonth { get; set; }
        public int DebitExpYear { get; set; }
    }
}
