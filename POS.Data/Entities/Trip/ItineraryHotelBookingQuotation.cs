using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ItineraryHotelBookingQuotation : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ItineraryHotelId { get; set; }
        public string? QuotationName { get; set; }
        public string? QuotationPath { get; set; }
        public string? TravelDeskNotes { get; set; }
        public string? RMNotes { get; set; }
    }
}
