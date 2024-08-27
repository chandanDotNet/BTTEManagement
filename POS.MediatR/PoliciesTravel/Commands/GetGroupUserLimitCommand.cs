using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Commands
{
    public class GetGroupUserLimitCommand : IRequest<ServiceResponse<GroupUserLimitList>>
    {
        public List<Guid> UersIds { get; set; }
    }
}
