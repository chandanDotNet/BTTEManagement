using BTTEM.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class GetUserInfoDetailsQuery : IRequest<UserInfoData>
    {
        public Guid UserId { get; set; }
    }
}
