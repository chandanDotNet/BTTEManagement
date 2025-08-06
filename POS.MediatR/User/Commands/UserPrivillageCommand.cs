using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.User.Commands
{
    public class UserPrivillageCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid RoleId { get; set; }
        public string ActionKey { get; set; }
    }
}
