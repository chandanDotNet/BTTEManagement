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
    public class UpdateItineraryTicketBookingQuotationCommand : IRequest<ServiceResponse<bool>>
    {
        public List<ItineraryTicketBookingQuotationDto> ItineraryTicketBookingQuotationList { get; set; }
    }
}
