using BTTEM.Data;
using BTTEM.Data.Dto;
using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class CancelTripItineraryHotelCommand : IRequest<ServiceResponse<bool>>
    {
        public List<CancelTripItineraryHotel> cancelTripItineraryHotel { get; set; }

    }

    public class CancelTripItineraryHotel
    {
        public string Type { get; set; }
        public Guid Id { get; set; }
        public string? NoOfTickets { get; set; }
        public string? NoOfRoom { get; set; }
        public bool? IsItinerary { get; set; }
        public bool? IsCancel { get; set; }
        public List<CancelTripItineraryHotelUserDto> cancelTripItineraryHotelUsers { get; set; }
       
    }
    //public class CancelTripItineraryHotelUser
    //{
    //    public Guid Id { get; set; }
    //    public Guid UserId { get; set; }
    //    public Guid TripItineraryId { get; set; }
    //    public bool IsCancelrequest { get; set; }

    //}
}
