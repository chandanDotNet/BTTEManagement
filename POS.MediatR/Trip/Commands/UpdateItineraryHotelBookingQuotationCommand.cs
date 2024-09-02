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
    public class UpdateItineraryHotelBookingQuotationCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public Guid ItineraryHotelId { get; set; }
        public string? QuotationName { get; set; }
        public string? QuotationPath { get; set; }
        public string? TravelDeskNotes { get; set; }
        public string? RMNotes { get; set; }
        public bool? IsQuotationUpload { get; set; }
    }
}
