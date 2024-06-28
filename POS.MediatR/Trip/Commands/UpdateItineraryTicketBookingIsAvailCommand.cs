using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateItineraryTicketBookingIsAvailCommand : IRequest<ServiceResponse<bool>>
    {
        public List<Guid> Id { get; set; }
    }
}
