using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddMMTTripCommand
    {
        public MMTTripRequest MMTTrip { get; set; }
    }

    //MMT TRIP REQUEST

    public class ApprovalDetails
    {
        public string approvalRequired { get; set; }
        public List<object> approverDetails { get; set; }
    }

    public class Child
    {
        public string count { get; set; }
        public List<object> age { get; set; }
    }

    public class DeviceDetails
    {
        public string platform { get; set; }
    }

    public class FLIGHT
    {
        public string serviceId { get; set; }
        public string tripType { get; set; }
        public string travelClass { get; set; }
        public PaxDetail paxDetails { get; set; }
        public List<JourneyDetail> journeyDetails { get; set; }
    }

    public class From
    {
        public string airportCode { get; set; }
        public string cityName { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
    }

    public class JourneyDetail
    {
        public From from { get; set; }
        public To to { get; set; }
        public string departureDate { get; set; }
        public string arrivalDate { get; set; }
    }

    public class PaxDetail
    {
        public string name { get; set; }
        public string email { get; set; }
        public string isPrimaryPax { get; set; }
        public string adult { get; set; }
        public Child child { get; set; }
        public string infant { get; set; }
    }

    public class ReasonForTravel
    {
        public string reason { get; set; }
    }

    public class MMTTripRequest
    {
        public DeviceDetails deviceDetails { get; set; }
        public TravellerDetails travellerDetails { get; set; }
        public Services services { get; set; }
        public List<HOTEL> HOTEL { get; set; }
        public ReasonForTravel reasonForTravel { get; set; }
        public ApprovalDetails approvalDetails { get; set; }
        public string trfId { get; set; }
    }

    public class Services
    {
        public List<FLIGHT> FLIGHT { get; set; }
    }

    public class To
    {
        public string airportCode { get; set; }
        public string cityName { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
    }

    public class TravellerDetails
    {
        public List<PaxDetail> paxDetails { get; set; }
    }

    public class HOTEL
    {
        public string serviceId { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public long checkin { get; set; }
        public long checkout { get; set; }
        public List<RoomDetailsPaxWise> roomDetailsPaxWise { get; set; }
    }
    public class RoomDetailsPaxWise
    {
        public int adult { get; set; }
        public Child child { get; set; }
        public int infant { get; set; }
    }
}
