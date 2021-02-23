using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication1.Identity
{
    public class AccessGroup : BaseEntity
    {
        public AccessGroup()
        {
            Roles = new List<AccessGroupRole>();
            Users = new List<AccessGroupUser>();
        }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public virtual List<AccessGroupRole> Roles { get; set; }

        public virtual List<AccessGroupUser> Users { get; set; }
    }
}
