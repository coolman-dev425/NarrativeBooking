using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
        }
        [StringLength(100)]
        public string DisplayName { get; set; }
        [StringLength(100)]
        public string Path { get; set; }
        [StringLength(100)]
        public string LongPath { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool IsNavigation { get; set; }
        public bool IsVisible { get; set; }
    }
}
