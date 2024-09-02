using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class CancelTripUserCommand : IRequest<ServiceResponse<bool>>
    {
        public string Type { get; set; }
        public Guid TripId { get; set; }
        public List<GroupTripDto> GroupTripsUsers { get; set; }

    }
}
