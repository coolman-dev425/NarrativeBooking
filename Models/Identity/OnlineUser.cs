using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication1.Identity
{

    public class OnlineUser
    {
        [Key]
        [StringLength(36)]
        public string UserId { get; set; }
        [StringLength(36)]
        public string ConnectionId { get; set; }
        [StringLength(36)]
        public string DivisionId { get; set; }
    }
}
