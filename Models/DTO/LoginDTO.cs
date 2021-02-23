using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Identity
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
