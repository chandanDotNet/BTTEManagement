using BTTEM.MediatR.PoliciesTravel.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;
using System;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;
using POS.API.Helpers;
using BTTEM.Data.Resources;
using System.Threading;

namespace BTTEM.API.Controllers.Trip
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController :  BaseController
    {
        readonly IMediator _mediator;

        public TripController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All Purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPurpose")]
        [ClaimCheck("TRP_VIEW_PURPOSE")]
        public async Task<IActionResult> GetAllPurpose()
        {
            var getAllPurposeQuery = new GetAllPurposeQuery
            {
               
            };
            var result = await _mediator.Send(getAllPurposeQuery);

            //var paginationMetadata = new
            //{
            //    totalCount = result.TotalCount,
            //    pageSize = result.PageSize,
            //    skip = result.Skip,
            //    totalPages = result.TotalPages
            //};
            //Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }


        /// <summary>
        ///  Create a Trip
        /// </summary>
        /// <param name="addTripCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("TRP_ADD_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> AddTripDetail(AddTripCommand addTripCommand)
        {
            GetNewTripNumberCommand getNewTripNumber=new GetNewTripNumberCommand();
            string TripNo = await _mediator.Send(getNewTripNumber);
            addTripCommand.TripNo = TripNo;
            var result = await _mediator.Send(addTripCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Trip
        /// </summary>
        /// <param name="updateTripCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [ClaimCheck("TRP_UPDATE_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> UpdateTripDetail(UpdateTripCommand updateTripCommand)
        {
            var result = await _mediator.Send(updateTripCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Trip.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ClaimCheck("TRP_DELETE_TRIP")]
        public async Task<IActionResult> DeleteTrip(Guid Id)
        {
            var deleteTripCommand = new DeleteTripCommand { Id = Id };
            var result = await _mediator.Send(deleteTripCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get All Trips
        /// </summary>

        /// <returns></returns>

        [HttpGet(Name ="GetAllTrip")]
        //[ClaimCheck("TRP_VIEW_TRIP")]
        public async Task<IActionResult> GetAllTrip([FromQuery] TripResource TripResource)
        {
            var getAllTripQuery = new GetAllTripQuery
            {
               TripResource=TripResource
            };
           
            var result = await _mediator.Send(getAllTripQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }


        /// <summary>
        ///  Create a Trip Itinerary
        /// </summary>
        /// <param name="addTripItineraryCommand"></param>
        /// <returns></returns>
        [HttpPost("AddTripItinerary")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> AddTripItinerary(AddTripItineraryCommand addTripItineraryCommand)
        {
            var result = await _mediator.Send(addTripItineraryCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Trip Itinerary
        /// </summary>
        /// <param name="updateTripItineraryCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTripItinerary")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> UpdateTripItinerary(UpdateTripItineraryCommand updateTripItineraryCommand)
        {
            var result = await _mediator.Send(updateTripItineraryCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Trips Itinerary
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetAllTripItinerary")]
        public async Task<IActionResult> GetAllTripItinerary(Guid? Id)
        {
            var getAllTripItineraryQuery = new GetAllTripItineraryQuery
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllTripItineraryQuery);

            //var paginationMetadata = new
            //{
            //    totalCount = result.TotalCount,
            //    pageSize = result.PageSize,
            //    skip = result.Skip,
            //    totalPages = result.TotalPages
            //};
            //Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }

        /// <summary>
        /// Delete Trip Itinerary.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTripItinerary/{Id}")]
        public async Task<IActionResult> DeleteTripItinerary(Guid Id)
        {
            var deleteTripItineraryCommand = new DeleteTripItineraryCommand { Id = Id };
            var result = await _mediator.Send(deleteTripItineraryCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Create a Trip Hotel Booking
        /// </summary>
        /// <param name="addTripHotelBookingCommand"></param>
        /// <returns></returns>
        [HttpPost("AddTripHotelBooking")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripHotelBookingDto))]
        public async Task<IActionResult> AddTripHotelBooking(AddTripHotelBookingCommand addTripHotelBookingCommand)
        {
            var result = await _mediator.Send(addTripHotelBookingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Trip Hotel Booking
        /// </summary>
        /// <param name="updateTripHotelBookingCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTripHotelBooking")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripHotelBookingDto))]
        public async Task<IActionResult> UpdateTripHotelBooking(UpdateTripHotelBookingCommand updateTripHotelBookingCommand)
        {
            var result = await _mediator.Send(updateTripHotelBookingCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Delete Trip Hotel Booking.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTripHotelBooking/{Id}")]
        public async Task<IActionResult> DeleteTripHotelBooking(Guid Id)
        {
            var deleteTripHotelBookingCommand = new DeleteTripHotelBookingCommand { Id = Id };
            var result = await _mediator.Send(deleteTripHotelBookingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Trips Hotel Booking
        /// </summary>

        /// <returns></returns>

        [HttpGet("GetAllTripHotelBooking")]
        public async Task<IActionResult> GetAllTripHotelBooking(Guid? Id)
        {
            var getAllTripHotelBookingQuery = new GetAllTripHotelBookingQuery
            {
                Id = Id
            };
            var result = await _mediator.Send(getAllTripHotelBookingQuery);

            //var paginationMetadata = new
            //{
            //    totalCount = result.TotalCount,
            //    pageSize = result.PageSize,
            //    skip = result.Skip,
            //    totalPages = result.TotalPages
            //};
            //Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(result);
        }
    }
}
