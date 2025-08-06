using System;

namespace POS.Data.Dto
{
    public class UserInfoToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string ConnectionId { get; set; }
        public Guid? CompanyAccountId { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AccountTeam { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
