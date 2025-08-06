using System;

namespace POS.Data.Dto
{
    public class UserRoleDto 
    {
        public Guid? UserId { get; set; }
        public Guid RoleId { get; set; }
        public RoleDto Role { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? RoleName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
