using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Security.Claims;
using WebApplication1.Identity;
using WebApplication1.Models;

namespace WebApplication1.Helper
{
    public class RoleStore : RoleStore<Role, BooksContext, int, UserRole, RoleClaim>
    {
        public RoleStore(BooksContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        protected override RoleClaim CreateRoleClaim(Role role, Claim claim)
        {
            throw new NotImplementedException();
        }
    }
}
