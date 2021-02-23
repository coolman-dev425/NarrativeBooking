using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Identity;

namespace WebApplication1.Models
{
    public class UserChatModel
    {
        public IEnumerable<User> FollowUsers { get; set; }
        public bool HasAvatar { get; set; }
    }
}
