using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Identity
{
    public class AccessGroupRole : BaseEntity
    {
        public int AccessGroupId { get; set; }
        public int RoleId { get; set; }
        public bool? IsAccessible { get; set; }
        public virtual Role Role { get; set; }
        public virtual AccessGroup AccessGroup { get; set; }
    }
}
