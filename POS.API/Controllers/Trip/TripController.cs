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
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

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
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly ICompanyAccountRepository _companyAccountRepository;
        public TripController(IMediator mediator, UserInfoToken userInfoToken, ITripRepository tripRepository, ITripItineraryRepository tripItineraryRepository, ITripHotelBookingRepository tripHotelBookingRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMapper mapper
            , IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IItineraryTicketBookingRepository itineraryTicketBookingRepository, ICompanyAccountRepository companyAccountRepository)
        {
            _mediator = mediator;
            _userInfoToken = userInfoToken;
            _tripRepository = tripRepository;
            _tripItineraryRepository = tripItineraryRepository;
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _itineraryTicketBookingRepository = itineraryTicketBookingRepository;
            _companyAccountRepository = companyAccountRepository;
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
            TripDetailsData tripDetailsData = new TripDetailsData();
            GetNewTripNumberCommand getNewTripNumber = new GetNewTripNumberCommand();
            string TripNo = await _mediator.Send(getNewTripNumber);
            addTripCommand.TripNo = TripNo;

            //===================

            //var trip = _tripRepository.All.Where(a => a.TripStarts <= addTripCommand.TripStarts && a.TripEnds >= addTripCommand.TripStarts && a.CreatedBy == Guid.Parse(_userInfoToken.Id)).ToList();
            //if (trip.Count > 0)
            //{
            //    var tripList = _mapper.Map<List<TripDto>>(trip);
            //    tripDetailsData.Data = tripList;
            //    tripDetailsData.status = false;
            //    tripDetailsData.StatusCode = 409;
            //    tripDetailsData.message = "Data already exists ";
            //    return Ok(tripDetailsData);
            //    //var ss= ServiceResponse<List<TripDto>>.ReturnResultWith402(trip10);               
            //    //ss.Errors = false;
            //    // var sss= ReturnFormattedResponse(ss); 
            //    //sss.ShapeData = 500;
            //    //return Ok(sss); 
            //    // return (IActionResult)ServiceResponse<List<TripDto>>.Return500();
            //}
            //var trip2 = _tripRepository.All.Where(a => a.TripStarts <= addTripCommand.TripEnds && a.TripEnds >= addTripCommand.TripEnds && a.CreatedBy == Guid.Parse(_userInfoToken.Id)).ToList();
            //if (trip2.Count > 0)
            //{
            //    var tripList1 = _mapper.Map<List<TripDto>>(trip2);
            //    tripDetailsData.Data = tripList1;
            //    tripDetailsData.status = false;
            //    tripDetailsData.StatusCode = 409;
            //    tripDetailsData.message = "Data already exists ";
            //    return Ok(tripDetailsData);
            //}
            ////===============

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
                    TripId = result.Data.Id,
                    TypeName = result.Data.Name,
                    SourceId = result.Data.CreatedBy,
                    Content = "New Trip Added By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = _userRepository.FindAsync(result.Data.CreatedBy).Result.ReportingTo.Value,
                };
                var notificationResult = await _mediator.Send(addNotificationCommand);

                string email = this._configuration.GetSection("AppSettings")["Email"];
                if (email == "Yes")
                {
                    if (addTripCommand.Status == "APPLIED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AddTrip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(userResult.ReportingTo.Value).Result;

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(result.Data.TripNo));
                            templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString(result.Data.Status));
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", Convert.ToString(result.Data.ModeOfTrip));
                            templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(result.Data.DepartmentName));
                            templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(result.Data.TripType));
                            templateBody = templateBody.Replace("{JOURNEY_DATE}", Convert.ToString(result.Data.TripStarts.ToString("dd MMMM yyyy")));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(result.Data.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(result.Data.DestinationCityName));
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(result.Data.PurposeFor));
                            templateBody = templateBody.Replace("{GROUP_TRIP}", Convert.ToString(result.Data.IsGroupTrip == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(result.Data.NoOfPerson == null ? "0" : result.Data.NoOfPerson));
                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "New Trip Request",
                                ToAddress = reportingHead.UserName,
                                CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                                userResult.UserName + ",travels@shyamsteel.com,bitan@shyamsteel.com" :
                                userResult.UserName + ",travels@shyamsteel.com,bitan@shyamsteel.com," + userResult.AlternateEmail,
                                UserName = defaultSmtp.UserName
                            });
                        }
                    }
                }
            }

            List<TripDto> tripDtoList = new List<TripDto>();
            tripDtoList.Add(result.Data);
            tripDetailsData.Data = tripDtoList;
            tripDetailsData.status = true;
            tripDetailsData.StatusCode = 200;
            tripDetailsData.message = "Trip added successfully";
            return Ok(tripDetailsData);

            //return ReturnFormattedResponse(result);
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
            TripDetailsData tripDetailsData = new TripDetailsData();
            List<TripDto> tripDtoList = new List<TripDto>();

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
            else
            {

                //tripDtoList.Add(result.Data); 
                tripDetailsData.Data = tripDtoList;
                tripDetailsData.status = false;
                tripDetailsData.StatusCode = 500;
                tripDetailsData.message = "Something went wrong!! please try again";
                return Ok(tripDetailsData);
            }

            //return ReturnFormattedResponse(result);

            //tripDtoList.Add(result.Data);
            tripDetailsData.Data = tripDtoList;
            tripDetailsData.status = true;
            tripDetailsData.StatusCode = 200;
            tripDetailsData.message = "Trip updated successfully";
            return Ok(tripDetailsData);
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
        ///  Create a Trip Itinerary Ticket Booking
        /// </summary>
        /// <param name="addItineraryTicketBookingCommand"></param>
        /// <returns></returns>
        [HttpPost("AddItineraryTicketBooking")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryTicketBookingDto))]
        public async Task<IActionResult> AddItineraryTicketBooking(AddItineraryTicketBookingCommand addItineraryTicketBookingCommand)
        {
            var result = await _mediator.Send(addItineraryTicketBookingCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var addTripTrackingCommand = new AddTripTrackingCommand() 
                //{
                //    TripId = result.Data.TripId,
                //    TripItineraryId = result.Data.Id,
                //    TripTypeName = result.Data.TripBy,
                //    ActionType = "Activity",
                //    Remarks = "Trip Itinerary Added For " + result.Data.TripBy + " By " + userResult.FirstName + " " + userResult.LastName,
                //    Status = "Trip Itinerary Added By " + userResult.FirstName + " " + userResult.LastName, 
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now,
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);


                //**Email Start**

                var tripItineraryDetails = await _tripItineraryRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == result.Data.TripItineraryId).FirstOrDefaultAsync();
                var tripDetails = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == tripItineraryDetails.TripId).FirstOrDefaultAsync();

                string email = this._configuration.GetSection("AppSettings")["Email"];
                if (email == "Yes")
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "TicketUpload.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(tripItineraryDetails.CreatedByUser.FirstName, " ", tripItineraryDetails.CreatedByUser.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(tripDetails.TripNo));
                        templateBody = templateBody.Replace("{TICKET}", Path.Combine(_webHostEnvironment.WebRootPath, "TravelDeskAttachments", result.Data.TicketReceiptPath));
                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Journey Tickets",
                            ToAddress = tripDetails.CreatedByUser.UserName,
                            CCAddress = string.IsNullOrEmpty(userResult.AlternateEmail) ?
                                        userResult.UserName :
                                        userResult.UserName + "," + userResult.AlternateEmail,
                            UserName = defaultSmtp.UserName
                        });
                    }
                }

                //**Email End**
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Trip Itinerary Ticket Booking
        /// </summary>
        /// <param name="updateItineraryTicketBookingCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateItineraryTicketBooking")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryTicketBookingDto))]
        public async Task<IActionResult> UpdateItineraryTicketBooking(UpdateItineraryTicketBookingCommand updateItineraryTicketBookingCommand)
        {
            var result = await _mediator.Send(updateItineraryTicketBookingCommand);
            //if (result.Data == true)
            //{
            //    var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
            //    var responseData = _tripItineraryRepository.FindAsync(updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id);

            //    var addTripTrackingCommand = new AddTripTrackingCommand()
            //    {
            //        TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id,
            //        TripItineraryId = Guid.Empty,
            //        TripTypeName = responseData.Result.TripBy,
            //        ActionType = "Activity",
            //        Remarks = "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
            //        ActionBy = Guid.Parse(_userInfoToken.Id),
            //        ActionDate = DateTime.Now
            //    };
            //    var response = await _mediator.Send(addTripTrackingCommand);
            //}

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Trip Itinerary Ticket Booking 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteItineraryTicketBooking/{Id}")]
        public async Task<IActionResult> DeleteItineraryTicketBooking(Guid Id)
        {
            var deleteTripItineraryCommand = new DeleteItineraryTicketBookingCommand { Id = Id };
            var result = await _mediator.Send(deleteTripItineraryCommand);
            if (result.Success)
            {
                //var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var responseData = _tripItineraryRepository.FindAsync(Id);
                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = responseData.Result.TripId,
                //    TripItineraryId = Id,
                //    ActionType = "Activity",
                //    Remarks = responseData.Result.TripBy + " Trip Itinerary Deleted By " + userResult.FirstName + " " + userResult.LastName,
                //    Status = "Trip Itinerary Deleted By " + userResult.FirstName + " " + userResult.LastName,
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Update a Trip Itinerary Ticket Booking IsAvail
        /// </summary>
        /// <param name="updateItineraryTicketBookingIsAvailCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateItineraryTicketBookingIsAvail")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryTicketBookingDto))]
        public async Task<IActionResult> UpdateItineraryTicketBookingIsAvail(UpdateItineraryTicketBookingIsAvailCommand updateItineraryTicketBookingIsAvailCommand)
        {
            var result = await _mediator.Send(updateItineraryTicketBookingIsAvailCommand);
            //if (result.Data == true)
            //{
            //    var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
            //    var responseData = _tripItineraryRepository.FindAsync(updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id);

            //    var addTripTrackingCommand = new AddTripTrackingCommand()
            //    {
            //        TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id,
            //        TripItineraryId = Guid.Empty,
            //        TripTypeName = responseData.Result.TripBy,
            //        ActionType = "Activity",
            //        Remarks = "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
            //        ActionBy = Guid.Parse(_userInfoToken.Id),
            //        ActionDate = DateTime.Now
            //    };
            //    var response = await _mediator.Send(addTripTrackingCommand);
            //}

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
            var deleteTripItineraryCommand = new DeleteAllTripItineraryCommand { Id = updateTripItineraryCommand.TripItinerary.FirstOrDefault().TripId };
            var resultDelete = await _mediator.Send(deleteTripItineraryCommand);

            var result = await _mediator.Send(updateTripItineraryCommand);
            if (result.Data == true)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var responseData = _tripItineraryRepository.FindAsync(updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id,
                    TripItineraryId = Guid.Empty,
                    TripTypeName = responseData.Result.TripBy,
                    ActionType = "Activity",
                    Remarks = "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        [HttpPut("RescheduleTripItineraryHotel")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> RescheduleTripItineraryHotel(RescheduleTripItineraryHotelCommand rescheduleTripItineraryHotelCommand)
        {

            var result = await _mediator.Send(rescheduleTripItineraryHotelCommand);
            if (result.Data == true)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var responseData = _tripItineraryRepository.FindAsync(rescheduleTripItineraryHotelCommand.Id);

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id,
                //    TripItineraryId = Guid.Empty,
                //    TripTypeName = responseData.Result.TripBy,
                //    ActionType = "Activity",
                //    Remarks = "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
            }

            return ReturnFormattedResponse(result);
        }

        [HttpPut("CancelRequestTripItineraryHotel")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> CancelRequestTripItineraryHotel(CancelTripItineraryHotelCommand cancelTripItineraryHotelCommand)
        {

            var result = await _mediator.Send(cancelTripItineraryHotelCommand);
            if (result.Data == true)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var responseData = _tripItineraryRepository.FindAsync(rescheduleTripItineraryHotelCommand.Id); 

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id,
                //    TripItineraryId = Guid.Empty,
                //    TripTypeName = responseData.Result.TripBy,
                //    ActionType = "Activity",
                //    Remarks = "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand); 
            }

            return ReturnFormattedResponse(result);
        }

        [HttpPut("CancelPacificUser")]
        //[ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> CancelPacificUser(CancelTripUserCommand cancelTripUserCommand)
        {

            var result = await _mediator.Send(cancelTripUserCommand);
            if (result.Data == true)
            {
                //var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //var responseData = _tripItineraryRepository.FindAsync(rescheduleTripItineraryHotelCommand.Id);

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id,
                //    TripItineraryId = Guid.Empty,
                //    TripTypeName = responseData.Result.TripBy,
                //    ActionType = "Activity",
                //    Remarks = "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
            }

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
                        Remarks =
                        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Trip Ticket Booked through Travel Desk For " + responseData.Result.TripBy
                        : "Trip Itinerary status " + updateTripItineraryBookStatusCommand.ApprovalStatus + " for " + responseData.Result.TripBy,
                        Status = !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Ticket Booked through Travel Desk - " + userResult.FirstName + " " + userResult.LastName
                        : "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
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
                        Remarks =
                        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Trip Ticket Booked By Travel Desk For Hotel" :
                        "Trip Itinerary status " + updateTripItineraryBookStatusCommand.ApprovalStatus,
                        Status =
                        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Ticket Hotel Booked through Travel Desk - " + userResult.FirstName + " " + userResult.LastName
                        : "Trip Hotel Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now
                    };
                    var response = await _mediator.Send(addTripTrackingCommand);
                }
            }
            return ReturnFormattedResponse(result);
        }


        ///Version 2.0
        /// <summary>
        /// All approval Trip Itinerary Book Status 
        /// </summary>
        /// <param name="updateAllTripItineraryBookStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateAllTripItineraryBookStatus")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> UpdateAllTripItinerary(UpdateAllTripItineraryBookStatusCommand updateAllTripItineraryBookStatusCommand)
        {
            var result = await _mediator.Send(updateAllTripItineraryBookStatusCommand);
            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                //if (updateAllTripItineraryBookStatusCommand.IsItinerary == true)
                //{
                //    var responseData = _tripItineraryRepository.FindAsync(updateAllTripItineraryBookStatusCommand.Id);

                //    var addTripTrackingCommand = new AddTripTrackingCommand()
                //    {
                //        TripId = updateTripItineraryBookStatusCommand.TripId.Value,
                //        TripItineraryId = updateTripItineraryBookStatusCommand.Id,
                //        TripTypeName = responseData.Result.TripBy,
                //        ActionType = "Activity",
                //        Remarks =
                //        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                //        "Trip Ticket Booked through Travel Desk For " + responseData.Result.TripBy
                //        : "Trip Itinerary status " + updateTripItineraryBookStatusCommand.ApprovalStatus + " for " + responseData.Result.TripBy,
                //        Status = !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                //        "Ticket Booked through Travel Desk - " + userResult.FirstName + " " + userResult.LastName
                //        : "Trip Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                //        ActionBy = Guid.Parse(_userInfoToken.Id),
                //        ActionDate = DateTime.Now
                //    };
                //    var response = await _mediator.Send(addTripTrackingCommand);
                //}
                //else
                //{
                //    var responseData = _tripHotelBookingRepository.FindAsync(updateTripItineraryBookStatusCommand.Id);
                //    var addTripTrackingCommand = new AddTripTrackingCommand()
                //    {
                //        TripId = updateTripItineraryBookStatusCommand.TripId.Value,
                //        TripItineraryId = updateTripItineraryBookStatusCommand.Id,
                //        TripTypeName = "Hotel",
                //        ActionType = "Activity",
                //        Remarks =
                //        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                //        "Trip Ticket Booked By Travel Desk For Hotel" :
                //        "Trip Itinerary status " + updateTripItineraryBookStatusCommand.ApprovalStatus,
                //        Status =
                //        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                //        "Ticket Hotel Booked through Travel Desk - " + userResult.FirstName + " " + userResult.LastName
                //        : "Trip Hotel Itinerary status updated by - " + userResult.FirstName + " " + userResult.LastName,
                //        ActionBy = Guid.Parse(_userInfoToken.Id),
                //        ActionDate = DateTime.Now
                //    };
                //    var response = await _mediator.Send(addTripTrackingCommand);
                //}
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
            if (result.Success)
            {
                DirectApproval(updateTripHotelBookingCommand.tripHotelBooking.FirstOrDefault().TripId.Value);
            }
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
                    TripId = updateTripRequestAdvanceMoneyCommand.Id,
                    TypeName = responseData.Result.Name,
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Request For Advance Money For Rs." + updateTripRequestAdvanceMoneyCommand.AdvanceMoney + " By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result.ReportingTo.Value,
                };

                var notificationResult = await _mediator.Send(addNotificationCommand);

                if (responseData.Result.Status == "APPLIED")
                {
                    //**Email Start**
                    string email = this._configuration.GetSection("AppSettings")["Email"];
                    if (email == "Yes")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoney.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var MoneyRequestBy = await _userRepository.FindAsync(responseData.Result.CreatedBy);


                        List<User> accountant = new List<User>();
                        string toAccount = string.Empty;
                        if (responseData.Result.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
                        {
                            if (responseData.Result.ProjectType == "Ongoing")
                            {
                                toAccount = "gpsss@shyamsteel.com";
                            }
                            else
                            {
                                toAccount = "raghavsss@shyamsteel.com";
                            }
                        }
                        else
                        {
                            accountant = await _userRepository.All.Include(u => u.UserRoles)
                            .Where(x => x.CompanyAccountId == MoneyRequestBy.CompanyAccountId).ToListAsync();

                            accountant =
                            accountant.Where(c => c.UserRoles.Select(cs => cs.RoleId)
                           .Contains(new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2"))).ToList();
                        }

                        if (string.IsNullOrEmpty(toAccount))
                        {
                            toAccount = string.Join(',', accountant.Select(x => x.UserName));
                        }

                        var reportingHead = await _userRepository.FindAsync(MoneyRequestBy.ReportingTo.Value);

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(MoneyRequestBy.FirstName, " ", MoneyRequestBy.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.Result.TripNo));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.Result.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.Result.DestinationCityName));
                            templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.Result.AdvanceMoney));
                            templateBody = templateBody.Replace("{STATUS}", Convert.ToString("applied"));
                            templateBody = templateBody.Replace("{COLOUR}", Convert.ToString("#00ff1a"));
                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Advance Money Approval",
                                ToAddress = reportingHead.UserName + "," + toAccount,
                                CCAddress = MoneyRequestBy.UserName,
                                UserName = defaultSmtp.UserName
                            });
                        }
                    }
                }
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
            bool TravelDesk = false;

            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
            if (userDetails.IsDirector)
            {
                updateTripStatusCommand.Approval = "APPROVED";
                updateTripStatusCommand.Status = "APPLIED";
            }
            var result = await _mediator.Send(updateTripStatusCommand);

            if (result.Success)
            {
                //var responseData = _tripRepository.FindAsync(updateTripStatusCommand.Id);
                var responseData = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == updateTripStatusCommand.Id).FirstOrDefaultAsync();
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                string StatusMessage = null, RemarksMessage = null;

                if (updateTripStatusCommand.Status == "ROLLBACK")
                {
                    RemarksMessage = responseData.Name + " Trip Rollback Updated By " + userResult.FirstName + " " + userResult.LastName;
                }
                else
                {
                    RemarksMessage = responseData.Name + " Trip Status Updated By " + userResult.FirstName + " " + userResult.LastName;
                }

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateTripStatusCommand.Id,
                    TripTypeName = responseData.Name,
                    //ActionType = "Tracker",
                    ActionType = "Activity",
                    Remarks = RemarksMessage,
                    Status = updateTripStatusCommand.Status,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };

                var response = await _mediator.Send(addTripTrackingCommand);

                var addNotificationCommand = new AddNotificationCommand()
                {
                    TripId = updateTripStatusCommand.Id,
                    TypeName = responseData.Name,
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Trip Status Changed By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = responseData.CreatedBy,
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
                        TravelDesk = true;
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
                                TripId = updateTripStatusCommand.Id,
                                TypeName = responseData.Name,
                                SourceId = Guid.Parse(_userInfoToken.Id),
                                Content = "Trip Status Changed By " + userResult.FirstName + " " + userResult.LastName,
                                UserId = userRoles.FirstOrDefault().UserId.Value,
                            };
                            var travelDeskNotificationResult = await _mediator.Send(travelDeskNotificationCommand);
                        }
                    }
                }

                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];

                if (email == "Yes")
                {
                    if (updateTripStatusCommand.Status == "APPLIED" && updateTripStatusCommand.Approval != "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AddTrip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = await _userRepository.FindAsync(userResult.ReportingTo.Value);

                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id).ToListAsync();
                        var hotel = await _tripHotelBookingRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id).ToListAsync();

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(responseData.CreatedByUser.FirstName, " ", responseData.CreatedByUser.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.TripNo));
                            templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString(responseData.Status));
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", Convert.ToString(itinerary.FirstOrDefault().TripBy));
                            templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(responseData.DepartmentName));
                            templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(responseData.TripType));
                            templateBody = templateBody.Replace("{JOURNEY_DATE}", Convert.ToString(responseData.TripStarts.ToString("dd MMMM yyyy")));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.DestinationCityName));
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(responseData.PurposeFor));
                            templateBody = templateBody.Replace("{GROUP_TRIP}", Convert.ToString(responseData.IsGroupTrip == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(responseData.NoOfPerson == null ? "0" : responseData.NoOfPerson));

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + updateTripStatusCommand.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + updateTripStatusCommand.Id);


                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Journey Request Updated",
                                ToAddress = string.IsNullOrEmpty(responseData.CreatedByUser.AlternateEmail) ?
                                responseData.CreatedByUser.UserName :
                                responseData.CreatedByUser.UserName + "," + responseData.CreatedByUser.AlternateEmail,
                                CCAddress = TravelDesk == false ?
                                reportingHead.UserName :
                                reportingHead.UserName + ",travels@shyamsteel.com,bitan@shyamsteel.com",
                                UserName = defaultSmtp.UserName
                            });
                        }






                        //**Email Start**

                        if (email == "Yes")
                        {
                            var amfilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoney.html");
                            var amdefaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                            var amMoneyRequestBy = await _userRepository.FindAsync(responseData.CreatedBy);

                            var amreportingHead = await _userRepository.FindAsync(userResult.ReportingTo.Value);

                            //List<User> accountant = new List<User>();
                            //string toAccount = string.Empty;
                            //if (responseData.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
                            //{
                            //    if (responseData.ProjectType == "Ongoing")
                            //    {
                            //        //toAccount = "gps@shyamsteel.com";
                            //        toAccount = "shubhajyoti.banerjee@shyamfuture.com";
                            //    }
                            //    else
                            //    {
                            //        //toAccount = "raghavs@shyamsteel.com";
                            //        toAccount = "abhishek.roy@shyamfuture.com";
                            //    }
                            //}
                            //else
                            //{

                            //    toAccount = "chiranjit.patra@shyamfuture.com";
                            //   // accountant = await _userRepository.All.Include(u => u.UserRoles)
                            //   // .Where(x => x.CompanyAccountId == amMoneyRequestBy.CompanyAccountId).ToListAsync();

                            //   // accountant =
                            //   // accountant.Where(c => c.UserRoles.Select(cs => cs.RoleId)
                            //   //.Contains(new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2"))).ToList();
                            //}

                            //if (string.IsNullOrEmpty(toAccount))
                            //{
                            //    toAccount = string.Join(',', accountant.Select(x => x.UserName));
                            //}

                            using (StreamReader sr = new StreamReader(amfilePath))
                            {
                                string templateBody = sr.ReadToEnd();
                                templateBody = templateBody.Replace("{NAME}", string.Concat(amMoneyRequestBy.FirstName, " ", amMoneyRequestBy.LastName));
                                templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                                templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.TripNo));
                                templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.SourceCityName));
                                templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.DestinationCityName));
                                templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.AdvanceMoney));
                                templateBody = templateBody.Replace("{STATUS}", Convert.ToString("applied"));
                                templateBody = templateBody.Replace("{COLOUR}", Convert.ToString("#00ff1a"));
                                EmailHelper.SendEmail(new SendEmailSpecification
                                {
                                    Body = templateBody,
                                    FromAddress = defaultSmtp.UserName,
                                    Host = defaultSmtp.Host,
                                    IsEnableSSL = defaultSmtp.IsEnableSSL,
                                    Password = defaultSmtp.Password,
                                    Port = defaultSmtp.Port,
                                    Subject = "Advance Money Approval",
                                    ToAddress = reportingHead.UserName,// + "," + toAccount,
                                    CCAddress = amMoneyRequestBy.UserName,
                                    UserName = defaultSmtp.UserName
                                });
                            }
                        }





                    }


                    if (updateTripStatusCommand.Approval == "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AddTrip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(responseData.CreatedByUser.ReportingTo.Value).Result;

                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id).ToListAsync();
                        var hotel = await _tripHotelBookingRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id).ToListAsync();

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(responseData.CreatedByUser.FirstName, " ", responseData.CreatedByUser.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.TripNo));
                            templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString("Approved"));
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", Convert.ToString(itinerary.FirstOrDefault().TripBy));
                            templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(responseData.DepartmentName));
                            templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(responseData.TripType));
                            templateBody = templateBody.Replace("{JOURNEY_DATE}", Convert.ToString(responseData.TripStarts.ToString("dd MMMM yyyy")));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.DestinationCityName));
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(responseData.PurposeFor));
                            templateBody = templateBody.Replace("{GROUP_TRIP}", Convert.ToString(responseData.IsGroupTrip == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(responseData.NoOfPerson == null ? "0" : responseData.NoOfPerson));

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + updateTripStatusCommand.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + updateTripStatusCommand.Id);

                            var ccUser = string.IsNullOrEmpty(responseData.CreatedByUser.AlternateEmail) ?
                                responseData.CreatedByUser.UserName :
                                responseData.CreatedByUser.UserName + "," + responseData.CreatedByUser.AlternateEmail;

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Journey Request Updated",
                                ToAddress = reportingHead.UserName,
                                CCAddress =
                                TravelDesk == false ?
                                ccUser :
                                ccUser + ",travels@shyamsteel.com,bitan@shyamsteel.com",
                                UserName = defaultSmtp.UserName
                            });
                        }
                    }
                }
                //**Email End**
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
                var responseData = await _tripRepository.FindAsync(updateStatusTripRequestAdvanceMoneyCommand.Id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateStatusTripRequestAdvanceMoneyCommand.Id,
                    TripTypeName = responseData.Name,
                    ActionType = "Activity",
                    Remarks = updateStatusTripRequestAdvanceMoneyCommand.Status == string.Empty ?
                    responseData.Name + " Requsted For Advance Money By " + userResult.FirstName + " " + userResult.LastName
                    : responseData.Name + " Requsted For Advance Money - Status Updated By " + userResult.FirstName + " " + userResult.LastName,
                    Status = updateStatusTripRequestAdvanceMoneyCommand.Status,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);


                var addNotificationCommand = new AddNotificationCommand()
                {
                    TripId = updateStatusTripRequestAdvanceMoneyCommand.Id,
                    TypeName = responseData.Name,
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Request For Advance Money " + updateStatusTripRequestAdvanceMoneyCommand.Status,
                    UserId = responseData.CreatedBy,
                };

                var notificationResult = await _mediator.Send(addNotificationCommand);

                if (updateStatusTripRequestAdvanceMoneyCommand.Status == "APPROVED")
                {
                    var userRoles = _userRoleRepository
                         .AllIncluding(c => c.User)
                         .Where(c => c.RoleId == new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2")
                         && c.User.CompanyAccountId ==
                         _userRepository.FindAsync(responseData.CreatedBy).Result.CompanyAccountId)
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
                        TripId = updateStatusTripRequestAdvanceMoneyCommand.Id,
                        TypeName = responseData.Name,
                        SourceId = Guid.Parse(_userInfoToken.Id),
                        Content = "Request For Advance Money " + updateStatusTripRequestAdvanceMoneyCommand.Status,
                        UserId = userRoles.FirstOrDefault().UserId.Value,
                    };

                    var accountManagerNotificationnotificationResult = await _mediator.Send(accountManagerNotificationCommand);
                }

                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                if (email == "Yes")
                {
                    if (updateStatusTripRequestAdvanceMoneyCommand.Status == "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoney.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var MoneyRequestBy = await _userRepository.FindAsync(responseData.CreatedBy);
                        var accountant = await _userRepository.All.Include(u => u.UserRoles)
                            .Where(x => x.CompanyAccountId == MoneyRequestBy.CompanyAccountId).ToListAsync();
                        accountant =
                            accountant.Where(c => c.UserRoles.Select(cs => cs.RoleId)
                            .Contains(new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2"))).ToList();
                        var toAddress = string.Join(',', accountant.Select(x => x.UserName));

                        var reportingHead = await _userRepository.FindAsync(MoneyRequestBy.ReportingTo.Value);

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(MoneyRequestBy.FirstName, " ", MoneyRequestBy.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.TripNo));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.DestinationCityName));
                            templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.AdvanceMoney));
                            templateBody = templateBody.Replace("{STATUS}", Convert.ToString(updateStatusTripRequestAdvanceMoneyCommand.Status));
                            templateBody = templateBody.Replace("{COLOUR}", updateStatusTripRequestAdvanceMoneyCommand.Status == "approved" ? Convert.ToString("#00ff1a") : Convert.ToString("#ff0000"));
                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Advance Money Approved",
                                ToAddress = toAddress,
                                CCAddress = reportingHead.UserName,
                                UserName = defaultSmtp.UserName
                            });
                        }
                    }
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

        /// <summary>
        /// Delete Group Trip.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteGroupTrip/{Id}")]
        public async Task<IActionResult> DeleteGroupTrip(Guid Id)
        {
            var deleteGroupTripCommand = new DeleteGroupTripCommand { Id = Id };
            var result = await _mediator.Send(deleteGroupTripCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        ///  Create a Trip Itinerary Ticket Booking Quotaion
        /// </summary>
        /// <param name="addItineraryTicketBookingQuotationCommand"></param>
        /// <returns></returns>
        [HttpPost("AddItineraryTicketBookingQuotaion")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryTicketBookingQuotationDto))]
        public async Task<IActionResult> AddItineraryTicketBookingQuotation(AddItineraryTicketBookingQuotationCommand addItineraryTicketBookingQuotationCommand)
        {
            var result = await _mediator.Send(addItineraryTicketBookingQuotationCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
                var reportingHead = await _userRepository.FindAsync(userResult.ReportingTo.Value);

                var tripItineraryDetails = await _tripItineraryRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == result.Data.TripItineraryId).FirstOrDefaultAsync();
                var tripDetails = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == tripItineraryDetails.TripId).FirstOrDefaultAsync();

                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                if (email == "Yes")
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "QuotationUpload.html");

                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(userResult.FirstName, " ", userResult.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(tripDetails.TripNo));
                        templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString(tripDetails.Status));
                        templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(tripDetails.DepartmentName));
                        templateBody = templateBody.Replace("{JOURNEY_DATE}", Convert.ToString(tripDetails.TripStarts.ToString("dd MMMM yyyy")));
                        templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(tripDetails.SourceCityName));
                        templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(tripDetails.DestinationCityName));
                        templateBody = templateBody.Replace("{QUOTATION}", "Quotation has been uploaded");
                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Quotation Upload",
                            ToAddress = tripDetails.CreatedByUser.UserName,
                            CCAddress = userResult.UserName + "," + reportingHead.UserName,
                            UserName = defaultSmtp.UserName
                        });
                    }
                }
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Update a Trip Itinerary Ticket Booking Quotaion 
        /// </summary>
        /// <param name="updateItineraryTicketBookingQuotationCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateItineraryTicketBookingQuotaion")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryTicketBookingQuotationDto))]
        public async Task<IActionResult> UpdateItineraryTicketBookingQuotation(UpdateItineraryTicketBookingQuotationCommand updateItineraryTicketBookingQuotationCommand)
        {
            var result = await _mediator.Send(updateItineraryTicketBookingQuotationCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Itinerary Ticket Booking Quotation.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteItineraryTicketBookingQuotation/{Id}")]
        public async Task<IActionResult> DeleteItineraryTicketBookingQuotation(Guid Id)
        {
            var deleteItineraryTicketBookingQuotationCommand = new DeleteItineraryTicketBookingQuotationCommand { Id = Id };
            var result = await _mediator.Send(deleteItineraryTicketBookingQuotationCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Create a Trip Itinerary Hotel Booking Quotaion
        /// </summary>
        /// <param name="addItineraryHotelBookingQuotationCommand"></param>
        /// <returns></returns>
        [HttpPost("AddItineraryHotelBookingQuotaion")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryHotelBookingQuotationDto))]
        public async Task<IActionResult> AddItineraryHotelBookingQuotation(AddItineraryHotelBookingQuotationCommand addItineraryHotelBookingQuotationCommand)
        {
            var result = await _mediator.Send(addItineraryHotelBookingQuotationCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
            }

            return ReturnFormattedResponse(result);
        }


        /// <summary>
        ///  Update a Trip Itinerary Hotel Booking Quotaion
        /// </summary>
        /// <param name="updateItineraryHotelBookingQuotationCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateItineraryHotelBookingQuotaion")]
        [Produces("application/json", "application/xml", Type = typeof(ItineraryHotelBookingQuotationDto))]
        public async Task<IActionResult> UpdateItineraryTicketBookingQuotation(UpdateItineraryHotelBookingQuotationCommand updateItineraryHotelBookingQuotationCommand)
        {
            var result = await _mediator.Send(updateItineraryHotelBookingQuotationCommand);

            if (result.Success)
            {
                var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;
            }

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Itinerary Hotel Booking Quotation.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteItineraryHotelBookingQuotation/{Id}")]
        public async Task<IActionResult> DeleteItineraryHotelBookingQuotation(Guid Id)
        {
            //int[] array = { 5, 3, 9, 2, 7 };
            //int max = array[0];
            //int secondMax = int.MinValue;
            //int thirdMax = int.MinValue;
            //for (int i = 1; i < array.Length; i++)
            //{
            //    if (array[i] > max)
            //    {
            //        thirdMax = secondMax;
            //        secondMax = max;
            //        max = array[i];
            //    }
            //    else if (array[i] > secondMax)
            //    {
            //        thirdMax = secondMax;
            //        secondMax = array[i];
            //    }
            //    else if (array[i] > thirdMax)
            //    {
            //        thirdMax = array[i];
            //    }
            //}

            //foreach (int num in array)
            //{
            //    if (num > max)
            //    {
            //        thirdMax = secondMax;
            //        secondMax = max;
            //        max = num;
            //    }
            //    else if (num > secondMax && num != max)
            //    {
            //        secondMax = num;
            //    }
            //}

            var deleteItineraryHotelBookingQuotationCommand = new DeleteItineraryHotelBookingQuotationCommand { Id = Id };
            var result = await _mediator.Send(deleteItineraryHotelBookingQuotationCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get Advance Money
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAdvanceMoney")]
        //[ClaimCheck("TRP_VIEW_TRIP")]
        public async Task<IActionResult> GetAdvanceMoney([FromQuery] AdvanceMoneyResource advanceMoneyResource)
        {
            var getAllAdvanceMoneyQuery = new GetAllAdvanceMoneyQuery
            {
                AdvanceMoneyResource = advanceMoneyResource
            };

            var result = await _mediator.Send(getAllAdvanceMoneyQuery);

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
        ///  Trip Request Advance Money Approval
        /// </summary>
        /// <param name="approvalTripRequestAdvanceMoneyCommand"></param>
        /// <returns></returns>
        [HttpPut("TripRequestAdvanceMoneyApproval")]
        [Produces("application/json", "application/xml", Type = typeof(TripDto))]
        public async Task<IActionResult> TripRequestAdvanceMoneyApproval(ApprovalTripRequestAdvanceMoneyCommand approvalTripRequestAdvanceMoneyCommand)
        {
            int Response = 0;
            ResponseData response = new ResponseData();
            UpdateApprovalTripRequestAdvanceMoneyCommand updateApprovalTripRequestAdvanceMoneyCommand = new UpdateApprovalTripRequestAdvanceMoneyCommand();
            foreach (var item in approvalTripRequestAdvanceMoneyCommand.UpdateApprovalTripRequestAdvanceMoneyCommand)
            {
                updateApprovalTripRequestAdvanceMoneyCommand = item;
                var result = await _mediator.Send(updateApprovalTripRequestAdvanceMoneyCommand);




                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                if (email == "Yes")
                {

                    var responseData = _tripRepository.FindAsync(item.Id);
                    var userResult = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result;

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoney.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    var MoneyRequestBy = await _userRepository.FindAsync(responseData.Result.CreatedBy);


                    List<User> accountant = new List<User>();
                    string toAccount = string.Empty;
                    if (responseData.Result.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
                    {
                        if (responseData.Result.ProjectType == "Ongoing")
                        {
                            toAccount = "gpsss@shyamsteel.com";
                        }
                        else
                        {
                            toAccount = "raghavsss@shyamsteel.com";                           
                        }
                    }
                    else
                    {

                        accountant = await _userRepository.All.Include(u => u.UserRoles)
                        .Where(x => x.CompanyAccountId == MoneyRequestBy.CompanyAccountId).ToListAsync();

                        accountant =
                        accountant.Where(c => c.UserRoles.Select(cs => cs.RoleId)
                       .Contains(new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2"))).ToList();
                    }

                    if (string.IsNullOrEmpty(toAccount))
                    {
                        toAccount = string.Join(',', accountant.Select(x => x.UserName));
                    }

                    var reportingHead = await _userRepository.FindAsync(MoneyRequestBy.ReportingTo.Value);

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(MoneyRequestBy.FirstName, " ", MoneyRequestBy.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.Result.TripNo));
                        templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.Result.SourceCityName));
                        templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.Result.DestinationCityName));
                        templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.Result.AdvanceMoney));
                        templateBody = templateBody.Replace("{STATUS}", Convert.ToString(item.AdvanceAccountApprovedStatus));
                        templateBody = templateBody.Replace("{COLOUR}", item.AdvanceAccountApprovedStatus == "approved" ? Convert.ToString("#00ff1a") : Convert.ToString("#ff0000"));
                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Advance Money Approval",
                            ToAddress = toAccount,
                            CCAddress = MoneyRequestBy.UserName + "," + reportingHead.UserName,
                            UserName = defaultSmtp.UserName
                        });
                    }
                }



                if (result.Success)
                {
                    Response = 1;
                }
            }
            if (Response > 0)
            {
                response.status = true;
                response.StatusCode = 200;
                //dashboardReportData.Data = result;

            }
            else
            {
                response.status = false;
                response.StatusCode = 500;
            }
            return Ok(response);
        }

        /// <summary>
        ///  Direct Approved
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        [NonAction]
        public async void DirectApproval(Guid tripId)
        {
            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
            if (userDetails.IsDirector)
            {
                //    UpdateTripStatusCommand updateTripStatusCommand = new UpdateTripStatusCommand()
                //    {
                //        Id = tripId,
                //        Approval = "APPROVED",
                //        Status = "APPLIED"
                //    };
                //    await UpdateTripStatus(updateTripStatusCommand);

                var entityItinerary = await _tripItineraryRepository.All.Where(x => x.TripId == tripId).ToListAsync();
                UpdateAllTripItineraryBookStatusCommand updateAllTripItineraryBookStatusCommand = new UpdateAllTripItineraryBookStatusCommand();

                List<AllTripItineraryBookStatus> allTripItineraryBookStatusList = new List<AllTripItineraryBookStatus>();

                if (entityItinerary.Count > 0 || entityItinerary != null)
                {
                    foreach (var item in entityItinerary)
                    {
                        allTripItineraryBookStatusList.Add(new AllTripItineraryBookStatus()
                        {
                            ApprovalStatus = "APPROVED",
                            ApprovalStatusDate = DateTime.Now,
                            Id = item.Id,
                            TripId = item.TripId,
                            IsItinerary = true
                        });
                    }
                }

                var entityHotel = await _tripHotelBookingRepository.All.Where(x => x.TripId == tripId).ToListAsync();
                if (entityHotel.Count > 0 || entityHotel != null)
                {
                    foreach (var item in entityHotel)
                    {
                        allTripItineraryBookStatusList.Add(new AllTripItineraryBookStatus()
                        {
                            ApprovalStatus = "APPROVED",
                            ApprovalStatusDate = DateTime.Now,
                            Id = item.Id,
                            TripId = item.TripId,
                            IsItinerary = false
                        });
                    }
                }

                updateAllTripItineraryBookStatusCommand.AllTripItineraryBookStatusList = allTripItineraryBookStatusList;

                if (entityItinerary.Count > 0 || entityItinerary != null || entityHotel.Count > 0 || entityHotel != null)
                {
                    await UpdateAllTripItinerary(updateAllTripItineraryBookStatusCommand);
                }
            }
        }
    }
}

