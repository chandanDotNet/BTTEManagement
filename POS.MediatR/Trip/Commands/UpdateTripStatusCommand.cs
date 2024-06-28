using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateTripStatusCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public string Status { get; set; }
        public string Approval { get; set; }
        public DateTime? CancellationDateTime { get; set; }
        public string CancellationConfirmation { get; set; }
        public string CancellationReason { get; set; }
        public string TravelDeskName { get; set; }
        public Guid? TravelDeskId { get; set; }
        public string JourneyNumber { get; set; }
    }
}
