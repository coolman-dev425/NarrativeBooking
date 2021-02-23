using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Identity
{
    public class UserRoleMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RoleId { get; set; }
        public int MenuId { get; set; }

        public ApplicationMenu MenuItem { get; set; }
        public Role Role { get; set; }
    }
}
