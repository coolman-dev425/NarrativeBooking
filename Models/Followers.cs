using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Followers")]
    public class Followers
    {
        public int UserId { get; set; }
        public int FollowUserId { get; set; }
    }
}
