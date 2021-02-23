using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("OfflineMessages")]
    public class MessageModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime SendTime { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
