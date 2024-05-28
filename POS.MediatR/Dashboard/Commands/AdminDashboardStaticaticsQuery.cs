using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AdminDashboardStaticaticsQuery : IRequest<AdminDashboardStatics>
    {
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public Guid? CompanyAccountId { get; set; }

    }
}
