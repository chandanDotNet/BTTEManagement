using BTTEM.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Commands
{
    public class DashboardStaticaticsQueryCommand : IRequest<DashboardData>
    {
        public Guid UserId { get; set; }
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public Guid? CompanyAccountId { get; set; }
        public bool IsMy { get; set; }= false;
    }
}
