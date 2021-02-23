using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Security.Claims;
using WebApplication1.Identity;
using WebApplication1.Models;

namespace WebApplication1.Helper
{
    public class UserStore : UserStore<User, Role, BooksContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        public UserStore(BooksContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
