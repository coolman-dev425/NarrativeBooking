using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplication1.Identity
{
    public class ApplicationMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Caption { get; set; } 
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Sequence { get; set; }
        public bool DisplayAsMenu { get; set; }

        public int? HeaderId { get; set; }
        public ApplicationMenu Header { get; set; }
        public IEnumerable<ApplicationMenu> Details { get; set; }
        public IEnumerable<UserRoleMenu> Roles { get; internal set; }
    }
}
