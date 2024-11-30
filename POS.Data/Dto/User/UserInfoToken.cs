using System;

namespace POS.Data.Dto
{
    public class UserInfoToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string ConnectionId { get; set; }
        public Guid? CompanyAccountId { get; set; }
        public string? AccountTeam { get; set; }
    }
}
