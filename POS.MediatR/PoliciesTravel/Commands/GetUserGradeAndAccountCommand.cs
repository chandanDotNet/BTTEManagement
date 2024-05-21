using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class GetUserGradeAndAccountCommand : IRequest<UserGradeAndAccountDto>
    {
        public Guid UserId { get; set; }
    }
}
