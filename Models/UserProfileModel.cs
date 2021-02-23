using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UserProfileModel
    {
        public IEnumerable<WishList> WishList { get; set; }
        public IEnumerable<Purchased> Purchased { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public bool HasAvatar { get; set; }
    }
}
