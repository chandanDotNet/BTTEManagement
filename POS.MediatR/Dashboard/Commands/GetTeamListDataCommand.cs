using BTTEM.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Commands
{
    public class GetTeamListDataCommand : IRequest<AllTeamListData>
    {
        public Guid ID { get; set; }

    }
}
