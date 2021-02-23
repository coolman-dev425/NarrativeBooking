using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Identity
{
    public class UserClaim : IdentityUserClaim<int>
    {
        [Key]
        public override int Id { get; set; }
    }
}
