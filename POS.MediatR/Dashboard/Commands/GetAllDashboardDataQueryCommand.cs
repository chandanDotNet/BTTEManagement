using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Commands
{
    public class GetAllDashboardDataQueryCommand : IRequest<AllDashboardData>
    {

        public Guid UserId { get; set; }
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public string DeviceKey { get; set; }
        public bool IsDeviceTypeAndroid { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
   
  
}
