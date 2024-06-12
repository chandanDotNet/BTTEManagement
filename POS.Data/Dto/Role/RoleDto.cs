using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class RoleDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
        public List<RoleClaimDto> RoleClaims { get; set; }

    }
}
