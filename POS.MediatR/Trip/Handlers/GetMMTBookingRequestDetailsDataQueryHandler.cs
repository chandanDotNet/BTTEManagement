using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Expense.Commands;
using BTTEM.MediatR.Expense.Handlers;
using BTTEM.MediatR.Trip.Commands;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class GetMMTBookingRequestDetailsDataQueryHandler : IRequestHandler<GetMMTBookingRequestDetailsDataQuery, MMTData>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetMMTBookingRequestDetailsDataQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetMMTBookingRequestDetailsDataQueryHandler(
            IMapper mapper,
            ILogger<GetMMTBookingRequestDetailsDataQueryHandler> logger,
            PathHelper pathHelper)
        {
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }
        public async Task<MMTData> Handle(GetMMTBookingRequestDetailsDataQuery request, CancellationToken cancellationToken)
        {
            MMTData mMTData = new MMTData();

            TripClassification tripClassification = new TripClassification();
            FromTripDetails fromTripDetails = new FromTripDetails();
            List<TripItineraries> tripItineraries = new List<TripItineraries>(); 

            string connectionString = _pathHelper.connectionStrings.Trim();

            //Action - 1
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MMTBookingRequestDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@TripId", request.TripId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tripClassification.TripBy = Convert.ToString(rdr["TripBy"]);
                    tripClassification.ServiceId = Convert.ToString(rdr["ServiceId"]);
                    tripClassification.TripType = Convert.ToString(rdr["TripType"]);
                    tripClassification.TravelClass = Convert.ToString(rdr["TravelClass"]);
                }
                con.Close();
            }

            //Action - 2
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MMTBookingRequestDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@TripId", request.TripId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tripItineraries.Add(new TripItineraries()
                    {
                        FromAirportCode = Convert.ToString(rdr["FromAirportCode"]),
                        FromCityName = Convert.ToString(rdr["FromCityName"]),
                        FromCountryCode = Convert.ToString(rdr["FromCountryCode"]),
                        FromCountryName = Convert.ToString(rdr["FromCountryName"]),
                        ToAirportCode = Convert.ToString(rdr["ToAirportCode"]),
                        ToCityName = Convert.ToString(rdr["ToCityName"]),
                        ToCountryCode = Convert.ToString(rdr["ToCountryCode"]),
                        ToCountryName = Convert.ToString(rdr["ToCountryName"]),
                        DepartureDate = Convert.ToString(rdr["DepartureDate"])
                    });
                }
                con.Close();
            }

            //Action - 3
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_MMTBookingRequestDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 3);
                cmd.Parameters.AddWithValue("@TripId", request.TripId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    fromTripDetails.ServiceId = Convert.ToString(rdr["ServiceId"]);
                    fromTripDetails.CityCode = Convert.ToString(rdr["CityCode"]);
                    fromTripDetails.CityName = Convert.ToString(rdr["CityName"]);
                    fromTripDetails.FromCountryCode = Convert.ToString(rdr["FromCountryCode"]);
                    fromTripDetails.FromCountryName = Convert.ToString(rdr["FromCountryName"]);
                    fromTripDetails.CheckIn = Convert.ToString(rdr["CheckIn"]);
                    fromTripDetails.CheckOut = Convert.ToString(rdr["CheckOut"]);
                }
                con.Close();
            }

            mMTData.tripClassification = tripClassification;
            mMTData.tripItineraries = tripItineraries;
            mMTData.fromTripDetails = fromTripDetails;

            return mMTData;
        }
    }
}