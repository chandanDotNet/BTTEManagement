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
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using System.Linq;
using POS.Data;
using System.Security.Claims;
using POS.Data.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using BTTEM.Repository;
using Microsoft.EntityFrameworkCore;
using POS.Repository;
using System.Security.Cryptography.X509Certificates;
using Azure.Core;
using System.ComponentModel.Design;
using BTTEM.Data.Dto;
using POS.Helper;
using AutoMapper;
using System.Collections.Generic;

namespace BTTEM.API.Controllers.Trip
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : BaseController
    {
        readonly IMediator _mediator;
        private readonly UserInfoToken _userInfoToken;
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        public TripController(IMediator mediator, UserInfoToken userInfoToken, ITripRepository tripRepository, ITripItineraryRepository tripItineraryRepository, ITripHotelBookingRepository tripHotelBookingRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _mediator = mediator;
            _userInfoToken = userInfoToken;
            _tripRepository = tripRepository;
            _tripItineraryRepository = tripItineraryRepository;
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPurpose")]
        //[ClaimCheck("TRP_VIEW_PURPOSE")]
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
        //[ClaimCheck("TRP_ADD_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> AddTripDetail(AddTripCommand addTripCommand)
        {
            GetNewTripNumberCommand getNewTripNumber = new GetNewTripNumberCommand();
            string TripNo = await _mediator.Send(getNewTripNumber);
            addTripCommand.TripNo = TripNo;

            //===================

            var trip=_tripRepository.All.Where(a=>a.TripStarts<=addTripCommand.TripStarts && a.TripEnds>= addTripCommand.TripStarts && a.CreatedBy== Guid.Parse(_userInfoToken.Id)).ToList();
            if(trip.Count>0)
            {
                var trip10 = _mapper.Map<List<TripDto>>(trip);
                var ss= ServiceResponse<List<TripDto>>.ReturnResultWith200(trip10);
               
                //ss.Errors = false;
                var sss= ReturnFormattedResponse(ss);
                //sss.ShapeData = 500;
                return Ok(sss);
            }
            var trip2 = _tripRepository.All.Where(a => a.TripStarts <= addTripCommand.TripEnds && a.TripEnds >= addTripCommand.TripEnds && a.CreatedBy == Guid.Parse(_userInfoToken.Id)).ToList();
            if (trip2.Count>0)
            {
                return Ok(trip2);
            }
            //===============

            var result = await _mediator.Send(addTripCommand);

            if (result.Success)
            {
                //Tracking
                var userResult = _userRepository.FindAsync(result.Data.CreatedBy).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = result.Data.Id,
                    TripTypeName = result.Data.Name,
                    ActionType = "Activity",
                    Remarks = result.Data.Name + " New Trip Added By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Trip Added By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = result.Data.CreatedBy,
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addTripTrackingCommand);

                var addNotificationCommand = new AddNotificationCommand()
                {
                    SourceId = result.Data.CreatedBy,
                    Content = "New Trip Added By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = _userRepository.FindAsync(result.Data.CreatedBy).Result.ReportingTo.Value,
                };
                var notificationResult = await _mediator.Send(addNotificationCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Trip
        /// </summary>
        /// <param name="updateTripCommand"></param>
        /// <returns></returns>
        [HttpPut]
        //[ClaimCheck("TRP_UPDATE_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> UpdateTripDetail(UpdateTripCommand updateTripCommand)
        {
            var result = await _mediator.Send(updateTripCommand);

            if (result.Success)
            {
                var responseData = await _tripRepository.FindAsync(updateTripCommand.Id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateTripCommand.Id,
                    TripTypeName = updateTripCommand.Name == string.Empty ? responseData.Name : updateTripCommand.Name,
                    ActionType = "Activity",
                    Remarks = updateTripCommand.Name == string.Empty ? responseData.Name : updateTripCommand.Name + "Trip Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Trip Updated By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }

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

            if (result.Success)
            {
                var responseData = await _tripRepository.FindAsync(Id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = Id,
                    TripTypeName = responseData.Name,
                    ActionType = "Activity",
                    Remarks = responseData.Name + "Trip Deleted  By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Trip Deleted  By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get All Trips
        /// </summary>

        /// <returns></returns>

        [HttpGet(Name = "GetAllTrip")]
        //[ClaimCheck("TRP_VIEW_TRIP")]
        public async Task<IActionResult> GetAllTrip([FromQuery] TripResource TripResource)
        {
            var getAllTripQuery = new GetAllTripQuery
            {
                TripResource = TripResource
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

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = result.Data.TripId,
                    TripItineraryId = result.Data.Id,
                    TripTypeName = result.Data.TripBy,
                    ActionType = "Activity",
                    Remarks = "Trip Itinerary Added For " + result.Data.TripBy + " By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Trip Itinerary Added By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }

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
        ///  Update a Trip Itinerary Book Status
        /// </summary>
        /// <param name="updateTripItineraryBookStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTripItineraryBookStatus")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> UpdateTripItinerary(UpdateTripItineraryBookStatusCommand updateTripItineraryBookStatusCommand)
        {
            var result = await _mediator.Send(updateTripItineraryBookStatusCommand);
            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                if (updateTripItineraryBookStatusCommand.IsItinerary == true)
                {
                    var responseData = _tripItineraryRepository.FindAsync(updateTripItineraryBookStatusCommand.Id);

                    var addTripTrackingCommand = new AddTripTrackingCommand()
                    {
                        TripId = updateTripItineraryBookStatusCommand.TripId.Value,
                        TripItineraryId = updateTripItineraryBookStatusCommand.Id,
                        TripTypeName = responseData.Result.TripBy,
                        ActionType = "Activity",
                        Remarks = "Trip Ticket Booked through Travel Desk For " + responseData.Result.TripBy,
                        Status = "Ticket Booked through Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now
                    };
                    var response = await _mediator.Send(addTripTrackingCommand);
                }
                else
                {
                    var responseData = _tripHotelBookingRepository.FindAsync(updateTripItineraryBookStatusCommand.Id);
                    var addTripTrackingCommand = new AddTripTrackingCommand()
                    {
                        TripId = updateTripItineraryBookStatusCommand.TripId.Value,
                        TripItineraryId = updateTripItineraryBookStatusCommand.Id,
                        TripTypeName = "Hotel",
                        ActionType = "Activity",
                        Remarks = "Trip Ticket Booked By Travel Desk For Hotel",
                        Status = "Ticket Booked through Travel Desk - " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now
                    };
                    var response = await _mediator.Send(addTripTrackingCommand);
                }
            }
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
            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var responseData = _tripItineraryRepository.FindAsync(Id);
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = responseData.Result.TripId,
                    TripItineraryId = Id,
                    ActionType = "Activity",
                    Remarks = responseData.Result.TripBy + " Trip Itinerary Deleted By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Trip Itinerary Deleted By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }
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


        /// <summary>
        ///  Update  Trip Request Advance Money
        /// </summary>
        /// <param name="updateTripRequestAdvanceMoneyCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTripRequestAdvanceMoney")]
        //[ClaimCheck("TRP_UPDATE_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> UpdateTripRequestAdvanceMoney(UpdateTripRequestAdvanceMoneyCommand updateTripRequestAdvanceMoneyCommand)
        {
            var result = await _mediator.Send(updateTripRequestAdvanceMoneyCommand);
            if (result.Success)
            {
                var responseData = _tripRepository.FindAsync(updateTripRequestAdvanceMoneyCommand.Id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateTripRequestAdvanceMoneyCommand.Id,
                    TripTypeName = responseData.Result.Name,
                    ActionType = "Activity",
                    Remarks = responseData.Result.Name + " Trip Request For Advance Money By " + userResult.FirstName + " " + userResult.LastName,
                    Status = "Trip Request For Advance Money By " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);

                var addNotificationCommand = new AddNotificationCommand()
                {
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Request For Advance Money For Rs." + updateTripRequestAdvanceMoneyCommand.AdvanceMoney + " By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result.ReportingTo.Value,
                };

                var notificationResult = await _mediator.Send(addNotificationCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update  Trip Status
        /// </summary>
        /// <param name="updateTripStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateTripStatus")]
        //[ClaimCheck("TRP_UPDATE_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> UpdateTripStatus(UpdateTripStatusCommand updateTripStatusCommand)
        {
            var result = await _mediator.Send(updateTripStatusCommand);

            if (result.Success)
            {
                var responseData = _tripRepository.FindAsync(updateTripStatusCommand.Id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateTripStatusCommand.Id,
                    TripTypeName = responseData.Result.Name,
                    ActionType = "Tracker",
                    Remarks = responseData.Result.Name + " Trip Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = updateTripStatusCommand.Status,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };

                var response = await _mediator.Send(addTripTrackingCommand);

                var addNotificationCommand = new AddNotificationCommand()
                {
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Trip Status Changed By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = responseData.Result.CreatedBy,
                };
                var notificationResult = await _mediator.Send(addNotificationCommand);

                if (updateTripStatusCommand.Approval == "APPROVED")
                {
                    var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id && x.BookTypeBy == "Travel Desk").ToListAsync();
                    var hotel = await _tripHotelBookingRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id && x.BookTypeBy == "Travel Desk").ToListAsync();

                    var companyId = Guid.Empty;

                    if (itinerary.Count > 0)
                    {
                        var requestUser = _userRepository.FindAsync(itinerary.FirstOrDefault().CreatedBy);
                        companyId = requestUser.Result.CompanyAccountId;

                    }
                    if (hotel.Count > 0)
                    {
                        var requestUser = _userRepository.FindAsync(hotel.FirstOrDefault().CreatedBy);
                        companyId = requestUser.Result.CompanyAccountId;
                    }

                    if (companyId != Guid.Empty)
                    {
                        var userRoles = _userRoleRepository
                           .AllIncluding(c => c.User)
                           .Where(c => c.RoleId == new Guid("F72616BE-260B-41BB-A4EE-89146622179A")
                           && c.User.CompanyAccountId == companyId)
                           .Select(cs => new UserRoleDto
                           {
                               UserId = cs.UserId,
                               RoleId = cs.RoleId,
                               UserName = cs.User.UserName,
                               FirstName = cs.User.FirstName,
                               LastName = cs.User.LastName
                           }).ToList();

                        if (userRoles.Count > 0)
                        {
                            var travelDeskNotificationCommand = new AddNotificationCommand()
                            {
                                SourceId = Guid.Parse(_userInfoToken.Id),
                                Content = "Trip Status Changed By " + userResult.FirstName + " " + userResult.LastName,
                                UserId = userRoles.FirstOrDefault().UserId.Value,
                            };
                            var travelDeskNotificationResult = await _mediator.Send(travelDeskNotificationCommand);
                        }
                    }
                }
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update Status Trip Request Advance Money
        /// </summary>
        /// <param name="updateStatusTripRequestAdvanceMoneyCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateStatusTripRequestAdvanceMoney")]
        //[ClaimCheck("TRP_UPDATE_TRIP")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> UpdateStatusTripRequestAdvanceMoney(UpdateStatusTripRequestAdvanceMoneyCommand updateStatusTripRequestAdvanceMoneyCommand)
        {
            var result = await _mediator.Send(updateStatusTripRequestAdvanceMoneyCommand);

            if (result.Success)
            {
                //var userId = (this.User.Claims.First(i => i.Type == "Id").Value);
                var responseData = _tripRepository.FindAsync(updateStatusTripRequestAdvanceMoneyCommand.Id);
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateStatusTripRequestAdvanceMoneyCommand.Id,
                    TripTypeName = responseData.Result.Name,
                    ActionType = "Activity",
                    Remarks = updateStatusTripRequestAdvanceMoneyCommand.Status == string.Empty ?
                    responseData.Result.Name + " Requsted For Advance Money By " + userResult.FirstName + " " + userResult.LastName
                    : responseData.Result.Name + " Requsted For Advance Money - Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = updateStatusTripRequestAdvanceMoneyCommand.Status,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);


                var addNotificationCommand = new AddNotificationCommand()
                {
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Request For Advance Money " + updateStatusTripRequestAdvanceMoneyCommand.Status,
                    UserId = responseData.Result.CreatedBy,
                };

                var notificationResult = await _mediator.Send(addNotificationCommand);

                if (updateStatusTripRequestAdvanceMoneyCommand.Status == "APPROVED")
                {
                    var userRoles = _userRoleRepository
                         .AllIncluding(c => c.User)
                         .Where(c => c.RoleId == new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2")
                         && c.User.CompanyAccountId ==
                         _userRepository.FindAsync(responseData.Result.CreatedBy).Result.CompanyAccountId)
                         .Select(cs => new UserRoleDto
                         {
                             UserId = cs.UserId,
                             RoleId = cs.RoleId,
                             UserName = cs.User.UserName,
                             FirstName = cs.User.FirstName,
                             LastName = cs.User.LastName
                         }).ToList();

                    var accountManagerNotificationCommand = new AddNotificationCommand()
                    {
                        SourceId = Guid.Parse(_userInfoToken.Id),
                        Content = "Request For Advance Money " + updateStatusTripRequestAdvanceMoneyCommand.Status,
                        UserId = userRoles.FirstOrDefault().UserId.Value,
                    };

                    var accountManagerNotificationnotificationResult = await _mediator.Send(accountManagerNotificationCommand);
                }
            }

            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Add Trip Tracking
        /// </summary>
        /// <param name="addTripTrackingCommand"></param>
        /// <returns></returns>
        [HttpPost("AddTripTracking")]
        [Produces("application/json", "application/xml", Type = typeof(TripTrackingDto))]
        public async Task<IActionResult> AddTripTracking([FromBody] AddTripTrackingCommand addTripTrackingCommand)
        {
            var result = await _mediator.Send(addTripTrackingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Trip Tracking
        /// </summary>
        /// <param name="tripTrackingResource"></param>
        /// <returns></returns>

        [HttpGet("GetTripTrackings")]
        public async Task<IActionResult> GetTripTrackings([FromQuery] TripTrackingResource tripTrackingResource)
        {
            var getAllTripTrackingQuery = new GetAllTripTrackingQuery
            {
                TripTrackingResource = tripTrackingResource
            };
            var result = await _mediator.Send(getAllTripTrackingQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }
    }
}
