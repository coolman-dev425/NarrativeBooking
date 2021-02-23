using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Identity
{

    public class AccessGroupUser : BaseEntity
    {
        public int UserId { get; set; }
        public int AccessGroupId { get; set; }
        public string CorporateId { get; set; }
        public string DivisionId { get; set; }
        public int TokenLimit { get; set; } // 0 = unlimited (no limit)
        public int TokenUsage { get; set; }
        public virtual User User { get; set; }
        public virtual AccessGroup AccessGroup { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
