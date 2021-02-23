using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("WishList")]
    public class WishList
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long WishListID { get; set; }
        public long BookId { get; set; }
        public string BookTitle { get; set; }
        public int CustomerId { get; set; }
    }
}
