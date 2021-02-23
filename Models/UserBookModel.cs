using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UserBookModel
    {
        public Book Book { get; set; }
        public int? UserId { get; set; }
        public bool IsInWishList { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsPurchased { get; set; }
    }
}
