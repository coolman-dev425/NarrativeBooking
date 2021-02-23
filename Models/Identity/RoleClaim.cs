using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace WebApplication1.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        /// <summary>
        /// Maximum user can access this menu per day / week / month / etc
        /// </summary>
        /// 
        [DefaultValue(0)]
        public int MaxAccess { get; set; }

        /// <summary>
        /// Status of this role  for current access
        /// True = User CAN access this menu
        /// False = User CAN NOT access this menu
        /// </summary>
        [DefaultValue(true)]
        public bool Status { get; set; }
    }
}
