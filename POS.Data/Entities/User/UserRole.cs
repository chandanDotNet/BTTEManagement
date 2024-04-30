using Microsoft.AspNetCore.Identity;
using System;

namespace POS.Data
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        //public  Role Roles { get; set; }
        //public Guid? UserId { get; set; }
        //public Guid RoleId { get; set; }
    }
}
