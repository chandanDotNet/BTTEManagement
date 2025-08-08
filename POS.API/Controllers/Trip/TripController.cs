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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using BTTEM.API.Service;
using System.Text;
using System.Text.RegularExpressions;
using BTTEM.API.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BTTEM.API.Controllers.Trip
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TripController : BaseController
    {
        readonly IMediator _mediator;
        private readonly UserInfoToken _userInfoToken;
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IItineraryTicketBookingRepository _itineraryTicketBookingRepository;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IItineraryHotelBookingQuotationRepository _itineraryHotelBookingQuotationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly ICompanyAccountRepository _companyAccountRepository;
        private readonly INotificationService _notificationService;
        public TripController(IMediator mediator, UserInfoToken userInfoToken, ITripRepository tripRepository, ITripItineraryRepository tripItineraryRepository, ITripHotelBookingRepository tripHotelBookingRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMapper mapper
            , IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IItineraryTicketBookingRepository itineraryTicketBookingRepository, ICompanyAccountRepository companyAccountRepository, IItineraryHotelBookingQuotationRepository itineraryHotelBookingQuotationRepository, INotificationService notificationService)
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
            _itineraryHotelBookingQuotationRepository = itineraryHotelBookingQuotationRepository;
            _notificationService = notificationService;
        }

        //Compare two Objects
        public static bool AreObjectsEqual<T>(T obj1, T obj2)
        {
            string json1 = JsonConvert.SerializeObject(obj1);
            string json2 = JsonConvert.SerializeObject(obj2);
            return json1 == json2;
        }

        /// <summary>
        /// Get All Purpose
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPurpose")]
        //[ClaimCheck("TRP_VIEW_PURPOSE")]
        public async Task<IActionResult> GetAllPurpose()
        {
            var getAllPurposeQuery = new GetAllPurposeQuery { };
            var result = await _mediator.Send(getAllPurposeQuery);
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

            var result = await _mediator.Send(addTripCommand);

            if (result.Success)
            {
                //Tracking
                //var userResult = await _userRepository.FindAsync(result.Data.CreatedBy);
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = result.Data.Id,
                    TripTypeName = result.Data.Name,
                    ActionType = "Activity",
                    Remarks = "Trip No. " + TripNo + " initialized.",
                    Status = "Added",
                    ActionBy = result.Data.CreatedBy,
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addTripTrackingCommand);
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
                //var responseData = await _tripRepository.FindAsync(updateTripCommand.Id);
                //var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = updateTripCommand.Id,
                //    TripTypeName = updateTripCommand.Name == string.Empty ? responseData.Name : updateTripCommand.Name,
                //    ActionType = "Activity",
                //    Remarks = "Trip No. " + responseData.TripNo + " has been updated.",
                //    Status = "Updated",
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now,
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = Id,
                    TripTypeName = responseData.Name,
                    ActionType = "Activity",
                    Remarks = "Trip No. " + responseData.TripNo + " deleted.",
                    Status = "Deleted",
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
                var trip = await _tripRepository.FindAsync(result.Data.TripId);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = result.Data.TripId,
                    TripItineraryId = result.Data.Id,
                    TripTypeName = result.Data.TripBy,
                    ActionType = "Activity",
                    Remarks = "Itinerary added - Trip No. " + trip.TripNo + ".",
                    Status = "Added",
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
                var tripItineraryTicketBooking = await _itineraryTicketBookingRepository.FindAsync(result.Data.Id);
                var tripItinerary = await _tripItineraryRepository.FindAsync(tripItineraryTicketBooking.TripItineraryId);
                var trip = await _tripRepository.FindAsync(tripItinerary.TripId);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = trip.Id,
                    TripItineraryId = tripItinerary.Id,
                    TripTypeName = trip.TripType,
                    ActionType = "Activity",
                    Remarks = "Journey ticket booked/uploaded - Trip No. " + trip.TripNo + ".",
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now,
                };
                var response = await _mediator.Send(addTripTrackingCommand);

                var tripItineraryDetails = await _tripItineraryRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == result.Data.TripItineraryId).FirstOrDefaultAsync();
                var tripDetails = await _tripRepository.AllIncluding(c => c.CreatedByUser).Where(x => x.Id == tripItineraryDetails.TripId).FirstOrDefaultAsync();


                //*** Start Notification Created User ***
                var reportingHead = await _userRepository.FindAsync(userResult.ReportingTo.Value);
                var addNotificationCommandReportingManager = new AddNotificationCommand()
                {
                    TripId = result.Data.Id,
                    TypeName = "Ticket Upload",
                    SourceId = tripDetails.CreatedByUser.Id,
                    Content = "Journey ticket booked/uploaded - the trip No. " + tripDetails.TripNo,
                    UserId = tripDetails.CreatedByUser.Id,
                    Redirection = "/trip/details/" + result.Data.Id
                };
                var notificationResultReportingManager = await _mediator.Send(addNotificationCommandReportingManager);
                //*** End Notification Created User ***


                //**Email Start**

                string email = this._configuration.GetSection("AppSettings")["Email"];
                string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];

                if (email == "Yes")
                {
                    string baseUrl = this._configuration.GetSection("Url")["BaseUrl"];
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "TicketUploadNewTemplate.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(tripItineraryDetails.CreatedByUser.FirstName, " ", tripItineraryDetails.CreatedByUser.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString(tripDetails.Status));
                        templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(tripDetails.TripNo));
                        templateBody = templateBody.Replace("{TICKET}", Path.Combine(baseUrl, "TravelDeskAttachments", result.Data.TicketReceiptPath));

                        templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + tripDetails.Id);
                        templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + tripDetails.Id + "/" + tripDetails.CreatedBy);

                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "Journey Tickets - " + tripDetails.TripNo,
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
            if (result.Data == true)
            {
                //var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                //var tripItineraryTicketBooking = await _itineraryTicketBookingRepository.FindAsync(updateItineraryTicketBookingCommand.Id);
                //var tripItinerary = await _tripItineraryRepository.FindAsync(tripItineraryTicketBooking.TripItineraryId);
                //var trip = await _tripRepository.FindAsync(tripItinerary.TripId);

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = trip.Id,
                //    TripItineraryId = tripItinerary.Id,
                //    TripTypeName = trip.TripType,
                //    ActionType = "Activity",
                //    Remarks = "Journey ticket modified for Trip No. " + trip.TripNo + ".",
                //    Status = "Updated",
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
            }

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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var tripItineraryTicketBooking = await _itineraryTicketBookingRepository.FindAsync(Id);
                var responseData = await _tripItineraryRepository.FindAsync(tripItineraryTicketBooking.TripItineraryId);
                var tripData = await _tripRepository.FindAsync(responseData.TripId);
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripItineraryId = Id,
                    ActionType = "Activity",
                    Remarks = " Journey ticket deleted - Trip No. " + tripData.TripNo + ".",
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
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
                //var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                //var responseData = await _tripItineraryRepository.FindAsync(updateTripItineraryCommand.TripItinerary.FirstOrDefault().Id);
                //var tripData = await _tripRepository.FindAsync(responseData.TripId);

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = updateTripItineraryCommand.TripItinerary.FirstOrDefault().TripId,
                //    TripItineraryId = Guid.Empty,
                //    TripTypeName = responseData.TripBy,
                //    ActionType = "Activity",
                //    Remarks = "Trip itinerary modified for trip No." + tripData.TripNo + ".",
                //    Status = "Updated",
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                var responseHotelData = await _tripHotelBookingRepository.FindAsync(rescheduleTripItineraryHotelCommand.Id);

                var responseItinareryData = await _tripItineraryRepository.FindAsync(rescheduleTripItineraryHotelCommand.Id);

                var tripBy = responseHotelData != null ? "Hotel" : responseItinareryData.TripBy;

                var tripData = await _tripRepository.FindAsync(responseHotelData == null ? responseItinareryData.TripId : responseHotelData.TripId);


                bool TravelDesk = false;
                var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == tripData.Id && x.BookTypeBy == "Travel Desk").ToListAsync();
                var hotel = await _tripHotelBookingRepository.All.Where(x => x.TripId == tripData.Id && x.BookTypeBy == "Travel Desk").ToListAsync();

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
                }


                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];

                string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];

                if (email == "Yes")
                {
                    if (tripData.Status == "APPLIED" && tripData.Approval != "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "Trip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();

                        var reportingHead = await _userRepository.FindAsync(tripData.CreatedByUser.ReportingTo.Value);

                        var itinerarys = await _tripItineraryRepository.All.Where(x => x.TripId == tripData.Id)
                            .OrderByDescending(x => x.TripBy).OrderBy(x => x.DepartureDate).ToListAsync();

                        var hotels = await _tripHotelBookingRepository.All.Where(x => x.TripId == tripData.Id).ToListAsync();

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(tripData.CreatedByUser.FirstName, " ", tripData.CreatedByUser.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(tripData.TripNo));
                            templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString(tripData.Status));
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", Convert.ToString(itinerarys.FirstOrDefault().TripBy));
                            templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(tripData.DepartmentName));
                            templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(tripData.TripType));
                            templateBody = templateBody.Replace("{JOURNEY_DATE}", Convert.ToString(tripData.TripStarts.ToString("dd MMMM yyyy")));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(tripData.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(tripData.DestinationCityName));
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(tripData.PurposeFor));
                            templateBody = templateBody.Replace("{GROUP_TRIP}", Convert.ToString(tripData.IsGroupTrip == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(tripData.NoOfPerson == null ? "1" : tripData.NoOfPerson));

                            var ca = await _companyAccountRepository.FindAsync(tripData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + tripData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + tripData.Id + "/" + tripData.CreatedBy);

                            string itineraryHtml = ItineraryHtml(itinerarys, tripData.TripType);

                            if (hotels.Count > 0)
                            {
                                string itineraryHotelHtml = ItineraryHotelHtml(hotels, "Hotel");
                                itineraryHtml = itineraryHtml + itineraryHotelHtml;
                            }

                            templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Journey Rescheduled - " + tripData.TripNo,
                                ToAddress = string.IsNullOrEmpty(tripData.CreatedByUser.AlternateEmail) ?
                                tripData.CreatedByUser.UserName :
                                tripData.CreatedByUser.UserName + "," + tripData.CreatedByUser.AlternateEmail,
                                CCAddress = TravelDesk == false ?
                                reportingHead.UserName :
                                reportingHead.UserName + ",travels@shyamsteel.com,bitan@shyamsteel.com",
                                UserName = defaultSmtp.UserName
                            });
                        }

                    }


                    if (tripData.Approval == "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "Trip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(tripData.CreatedByUser.ReportingTo.Value).Result;

                        var itinerarys = await _tripItineraryRepository.All.Where(x => x.TripId == tripData.Id)
                            .OrderByDescending(x => x.TripBy).OrderBy(x => x.DepartureDate).ToListAsync();

                        var hotels = await _tripHotelBookingRepository.All.Where(x => x.TripId == tripData.Id).ToListAsync();

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(tripData.CreatedByUser.FirstName, " ", tripData.CreatedByUser.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(tripData.TripNo));
                            templateBody = templateBody.Replace("{TRIP_STATUS}", Convert.ToString("Approved"));
                            templateBody = templateBody.Replace("{MODE_OF_TRIP}", Convert.ToString(itinerarys.FirstOrDefault().TripBy));
                            templateBody = templateBody.Replace("{DEPARTMENT}", Convert.ToString(tripData.DepartmentName));
                            templateBody = templateBody.Replace("{TRIP_TYPE}", Convert.ToString(tripData.TripType));
                            templateBody = templateBody.Replace("{JOURNEY_DATE}", Convert.ToString(tripData.TripStarts.ToString("dd MMMM yyyy")));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(tripData.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(tripData.DestinationCityName));
                            templateBody = templateBody.Replace("{JOURNEY_PURPOSE}", Convert.ToString(tripData.PurposeFor));
                            templateBody = templateBody.Replace("{GROUP_TRIP}", Convert.ToString(tripData.IsGroupTrip == true ? "Yes" : "No"));
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(tripData.NoOfPerson == null ? "1" : tripData.NoOfPerson));

                            var ca = await _companyAccountRepository.FindAsync(tripData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + tripData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + tripData.Id + "/" + tripData.CreatedBy);

                            string itineraryHtml = ItineraryHtml(itinerarys, tripData.TripType);

                            if (hotels.Count > 0)
                            {
                                string itineraryHotelHtml = ItineraryHotelHtml(hotels, "Hotel");
                                itineraryHtml = itineraryHtml + itineraryHotelHtml;
                            }

                            templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                            var ccUser = string.IsNullOrEmpty(tripData.CreatedByUser.AlternateEmail) ?
                                tripData.CreatedByUser.UserName :
                                tripData.CreatedByUser.UserName + "," + tripData.CreatedByUser.AlternateEmail;

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Journey Rescheduled - " + tripData.TripNo,
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



                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripItineraryId = Guid.Empty,
                    TripTypeName = tripData.TripType,
                    ActionType = "Activity",
                    Remarks = tripBy + " Booking rescheduled - Trip No. " + tripData.TripNo,
                    Status = "Rescheduled",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var responseData = await _tripItineraryRepository.FindAsync(cancelTripItineraryHotelCommand.cancelTripItineraryHotel.FirstOrDefault().Id);
                var tripData = await _tripRepository.FindAsync(responseData.TripId);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripItineraryId = Guid.Empty,
                    TripTypeName = tripData.TripType,
                    ActionType = "Activity",
                    Remarks = "Hotel booking cancelled - Trip No." + tripData.TripNo,
                    Status = "Cancelled",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var responseData = await _tripRepository.FindAsync(cancelTripUserCommand.TripId);

                List<string> cancelUser = new List<string>();
                foreach (var item in cancelTripUserCommand.GroupTripsUsers)
                {
                    var specificUser = await _userRepository.FindAsync(item.UserId);
                    cancelUser.Add(specificUser.FirstName + " " + specificUser.LastName);
                }
                var tt = cancelUser.Count() > 1 ? " have" : " has";
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = cancelTripUserCommand.TripId,
                    TripItineraryId = Guid.Empty,
                    TripTypeName = responseData.TripType,
                    ActionType = "Activity",
                    Remarks = string.Join(',', cancelUser) + tt + " been removed from Trip No. " + responseData.TripNo,
                    Status = "Removed",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                if (updateTripItineraryBookStatusCommand.IsItinerary == true)
                {
                    var responseData = await _tripItineraryRepository.FindAsync(updateTripItineraryBookStatusCommand.Id);
                    var tripData = await _tripRepository.FindAsync(updateTripItineraryBookStatusCommand.TripId.Value);

                    var addTripTrackingCommand = new AddTripTrackingCommand()
                    {
                        TripId = updateTripItineraryBookStatusCommand.TripId.Value,
                        TripItineraryId = updateTripItineraryBookStatusCommand.Id,
                        TripTypeName = tripData.TripType,
                        ActionType = "Activity",
                        Remarks =
                        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Trip ticket booked by travel desk - Trip No. " + tripData.TripNo
                        : "Trip itinerary status has been " + updateTripItineraryBookStatusCommand.ApprovalStatus + " - " + tripData.TripNo,
                        Status = !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Travel Desk" : "Updated",
                        ActionBy = Guid.Parse(_userInfoToken.Id),
                        ActionDate = DateTime.Now
                    };
                    var response = await _mediator.Send(addTripTrackingCommand);
                }
                else
                {
                    var responseData = await _tripHotelBookingRepository.FindAsync(updateTripItineraryBookStatusCommand.Id);
                    var tripData = await _tripRepository.FindAsync(responseData.TripId);

                    var addTripTrackingCommand = new AddTripTrackingCommand()
                    {
                        TripId = updateTripItineraryBookStatusCommand.TripId.Value,
                        TripItineraryId = updateTripItineraryBookStatusCommand.Id,
                        TripTypeName = "Hotel",
                        ActionType = "Activity",
                        Remarks =
                        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Hotel booked by travel desk - Trip No. " + tripData.TripNo :
                        "Hotel booking status updated to " + updateTripItineraryBookStatusCommand.ApprovalStatus + " - Trip No. " + tripData.TripNo,
                        Status =
                        !string.IsNullOrEmpty(updateTripItineraryBookStatusCommand.BookStatus) ?
                        "Travel Desk" : "Update",
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                foreach (var item in updateAllTripItineraryBookStatusCommand.AllTripItineraryBookStatusList)
                {
                    if (item.IsItinerary == true)
                    {
                        var responseData = await _tripItineraryRepository.FindAsync(item.Id);
                        var tripData = await _tripRepository.FindAsync(item.TripId.Value);

                        var addTripTrackingCommand = new AddTripTrackingCommand()
                        {
                            TripId = tripData.Id,
                            TripItineraryId = responseData.Id,
                            TripTypeName = tripData.TripType,
                            ActionType = "Activity",
                            Remarks =
                            !string.IsNullOrEmpty(item.BookStatus) ?
                            "Trip ticket booked by travel desk - Trip No. " + tripData.TripNo
                            : "Trip ticket booking status updated to " + item.ApprovalStatus,
                            Status = !string.IsNullOrEmpty(item.BookStatus) ?
                            "Travel Desk" : "Updated",
                            ActionBy = Guid.Parse(_userInfoToken.Id),
                            ActionDate = DateTime.Now
                        };
                        var response = await _mediator.Send(addTripTrackingCommand);
                    }
                    else
                    {
                        var responseData = await _tripHotelBookingRepository.FindAsync(item.Id);
                        var tripData = await _tripRepository.FindAsync(item.TripId.Value);

                        var addTripTrackingCommand = new AddTripTrackingCommand()
                        {
                            TripId = tripData.Id,
                            TripItineraryId = responseData.Id,
                            TripTypeName = tripData.TripType,
                            ActionType = "Activity",
                            Remarks =
                            !string.IsNullOrEmpty(item.BookStatus) ?
                            "Hotel booked by travel desk - Trip No. " + tripData.TripNo :
                            "Hotel booking status updated to " + item.ApprovalStatus,
                            Status =
                            !string.IsNullOrEmpty(item.BookStatus) ?
                            "Travel Desk" : "Updated",
                            ActionBy = Guid.Parse(_userInfoToken.Id),
                            ActionDate = DateTime.Now
                        };
                        var response = await _mediator.Send(addTripTrackingCommand);
                    }
                }
            }
            return ReturnFormattedResponse(result);
        }


        ///Version 2.0
        /// <summary>
        /// All approval Trip Itinerary Book Status For Director
        /// </summary>
        /// <param name="updateAllTripItineraryBookStatusForDirectorCommand"></param>
        /// <returns></returns>
        [HttpPut("UpdateAllTripItineraryBookStatusForDirector")]
        [Produces("application/json", "application/xml", Type = typeof(TripItineraryDto))]
        public async Task<IActionResult> UpdateAllTripItineraryForDirector(UpdateAllTripItineraryBookStatusForDirectorCommand updateAllTripItineraryBookStatusForDirectorCommand)
        {
            DashboardReportData dashboardReportData = new DashboardReportData();
            int Response = 0;
            foreach (var item in updateAllTripItineraryBookStatusForDirectorCommand.AllTripItineraryBookStatusList)
            {
                var result = await _mediator.Send(item);
                if (result.Success)
                {
                    Response = 1;
                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                    //var TrippID = item.TripId;
                    UpdateTripStatusCommand updateTripStatusCommand = new UpdateTripStatusCommand();
                    updateTripStatusCommand.Approval = item.ApprovalStatus;
                    updateTripStatusCommand.Id= item.TripId.Value;
                    var resultTrip = await _mediator.Send(updateTripStatusCommand);

                    //foreach (var item in updateAllTripItineraryBookStatusCommand.AllTripItineraryBookStatusList)
                    //{
                    //    if (item.IsItinerary == true)
                    //    {
                    //        var responseData = await _tripItineraryRepository.FindAsync(item.Id);
                    //        var tripData = await _tripRepository.FindAsync(item.TripId.Value);

                    //        var addTripTrackingCommand = new AddTripTrackingCommand()
                    //        {
                    //            TripId = tripData.Id,
                    //            TripItineraryId = responseData.Id,
                    //            TripTypeName = tripData.TripType,
                    //            ActionType = "Activity",
                    //            Remarks =
                    //            !string.IsNullOrEmpty(item.BookStatus) ?
                    //            "Trip ticket booked by travel desk - Trip No. " + tripData.TripNo
                    //            : "Trip ticket booking status updated to " + item.ApprovalStatus,
                    //            Status = !string.IsNullOrEmpty(item.BookStatus) ?
                    //            "Travel Desk" : "Updated",
                    //            ActionBy = Guid.Parse(_userInfoToken.Id),
                    //            ActionDate = DateTime.Now
                    //        };
                    //        var response = await _mediator.Send(addTripTrackingCommand);
                    //    }
                    //    else
                    //    {
                    //        var responseData = await _tripHotelBookingRepository.FindAsync(item.Id);
                    //        var tripData = await _tripRepository.FindAsync(item.TripId.Value);

                    //        var addTripTrackingCommand = new AddTripTrackingCommand()
                    //        {
                    //            TripId = tripData.Id,
                    //            TripItineraryId = responseData.Id,
                    //            TripTypeName = tripData.TripType,
                    //            ActionType = "Activity",
                    //            Remarks =
                    //            !string.IsNullOrEmpty(item.BookStatus) ?
                    //            "Hotel booked by travel desk - Trip No. " + tripData.TripNo :
                    //            "Hotel booking status updated to " + item.ApprovalStatus,
                    //            Status =
                    //            !string.IsNullOrEmpty(item.BookStatus) ?
                    //            "Travel Desk" : "Updated",
                    //            ActionBy = Guid.Parse(_userInfoToken.Id),
                    //            ActionDate = DateTime.Now
                    //        };
                    //        var response = await _mediator.Send(addTripTrackingCommand);
                    //    }
                    //}
                }

            }
            if (Response > 0)
            {
                dashboardReportData.status = true;
                dashboardReportData.StatusCode = 200;
                dashboardReportData.message = "Approved Successfully";
                //dashboardReportData.Data = result;
            }
            else
            {
                dashboardReportData.status = false;
                dashboardReportData.StatusCode = 500;
                //dashboardReportData.Data = result;
            }
            return Ok(dashboardReportData);
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var responseData = await _tripItineraryRepository.FindAsync(Id);
                var tripData = await _tripRepository.FindAsync(responseData.TripId);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripItineraryId = responseData.Id,
                    ActionType = "Activity",
                    Remarks = "Trip itinerary deleted - Trip No. " + tripData.TripNo,
                    Status = "Deleted",
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
                if (updateTripHotelBookingCommand.tripHotelBooking.FirstOrDefault().TripId.HasValue)
                {
                    DirectApproval(updateTripHotelBookingCommand.tripHotelBooking.FirstOrDefault().TripId.Value);
                }
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
            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var tripHotelBooking = await _tripHotelBookingRepository.FindAsync(Id);
                var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripItineraryId = Guid.Empty,
                    ActionType = "Activity",
                    Remarks = "Hotel deleted - Trip No. " + tripData.TripNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }

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
                var responseData = await _tripRepository.FindAsync(updateTripRequestAdvanceMoneyCommand.Id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateTripRequestAdvanceMoneyCommand.Id,
                    TripTypeName = responseData.TripType,
                    ActionType = "Activity",
                    Remarks = "Advance money modified - Trip No. " + responseData.TripNo,
                    Status = "Updated",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);

                var addNotificationCommand = new AddNotificationCommand()
                {
                    TripId = updateTripRequestAdvanceMoneyCommand.Id,
                    TypeName = responseData.Name,
                    SourceId = Guid.Parse(_userInfoToken.Id),
                    Content = "Advance Money requested For Rs." + updateTripRequestAdvanceMoneyCommand.AdvanceMoney + " By " + userResult.FirstName + " " + userResult.LastName,
                    UserId = _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id)).Result.ReportingTo.Value,
                };

                var notificationResult = await _mediator.Send(addNotificationCommand);

                if (responseData.Status == "APPLIED")
                {
                    //**Email Start**
                    string email = this._configuration.GetSection("AppSettings")["Email"];
                    string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];
                    if (email == "Yes")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoneyNewTemplate.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var MoneyRequestBy = await _userRepository.FindAsync(responseData.CreatedBy);


                        List<User> accountant = new List<User>();
                        string toAccount = string.Empty;
                        if (responseData.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
                        {
                            if (responseData.ProjectType == "Ongoing")
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
                            // accountant = await _userRepository.All.Include(u => u.UserRoles)
                            // .Where(x => x.CompanyAccountId == MoneyRequestBy.CompanyAccountId).ToListAsync();

                            // accountant =
                            // accountant.Where(c => c.UserRoles.Select(cs => cs.RoleId)
                            //.Contains(new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2"))).ToList();

                            var accountTeam = _companyAccountRepository.All.Where(x => x.Id == MoneyRequestBy.CompanyAccountId).FirstOrDefault().AccountTeam;

                            toAccount = this._configuration.GetSection(accountTeam)["UserEmail"];
                        }

                        //if (string.IsNullOrEmpty(toAccount))
                        //{
                        //    toAccount = string.Join(',', accountant.Select(x => x.UserName));
                        //}

                        var reportingHead = await _userRepository.FindAsync(MoneyRequestBy.ReportingTo.Value);

                        using (StreamReader sr = new StreamReader(filePath))
                        {
                            string templateBody = sr.ReadToEnd();
                            templateBody = templateBody.Replace("{NAME}", string.Concat(MoneyRequestBy.FirstName, " ", MoneyRequestBy.LastName));
                            templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                            templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.TripNo));
                            templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.SourceCityName));
                            templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.DestinationCityName));
                            templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.AdvanceAccountApprovedAmount != null ? responseData.AdvanceAccountApprovedAmount : responseData.AdvanceMoneyApprovedAmount == null ? 0 : responseData.AdvanceMoneyApprovedAmount));
                            templateBody = templateBody.Replace("{REQUEST_MONEY}", Convert.ToString(responseData.AdvanceMoney));
                            templateBody = templateBody.Replace("{STATUS}", Convert.ToString("applied"));
                            templateBody = templateBody.Replace("{COLOUR}", Convert.ToString("#00ff1a"));

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + responseData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + responseData.Id + "/" + responseData.CreatedBy);


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

                        //*** Start Notification Money Requested By ***
                        var addNotificationCommandMoneyRequestBy = new AddNotificationCommand()
                        {
                            TripId = responseData.Id,
                            TypeName = "Advance Money Approval",
                            SourceId = MoneyRequestBy.Id,
                            Content = "Advance Money Approval - Trip No. " + responseData.TripNo,
                            UserId = MoneyRequestBy.Id,
                            Redirection = "/trip/details/" + responseData.Id
                        };
                        var notificationResultMoneyRequestBy = await _mediator.Send(addNotificationCommandMoneyRequestBy);
                        //*** End Notification Money Requested By ***

                        //*** Start Notification Reporting Manager ***
                        var addNotificationCommandUser = new AddNotificationCommand()
                        {
                            TripId = responseData.Id,
                            TypeName = "Advance Money Approval",
                            SourceId = reportingHead.Id,
                            Content = "Advance Money Approval - Trip No. " + responseData.TripNo,
                            UserId = reportingHead.Id,
                            Redirection = "/trip/details/" + responseData.Id
                        };
                        var notificationResultUser = await _mediator.Send(addNotificationCommandUser);
                        //*** End Notification Reporting Manager ***


                        //*** Start Notification Accounts ***
                        foreach (var acc in accountant)
                        {
                            var addNotificationCommandAccounts = new AddNotificationCommand()
                            {
                                TripId = responseData.Id,
                                TypeName = "Advance Money Approval",
                                SourceId = acc.Id,
                                Content = "Advance Money Approval - Trip No. " + responseData.TripNo,
                                UserId = acc.Id,
                                Redirection = "/trip/details/" + responseData.Id
                            };
                            var notificationResultAccounts = await _mediator.Send(addNotificationCommandAccounts);
                        }
                        //*** End Notification Accounts ***
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                string StatusMessage = null, RemarksMessage = null;

                if (updateTripStatusCommand.Status == "ROLLBACK")
                {
                    RemarksMessage = "Trip rolled-back " + responseData.TripNo;
                }
                else
                {
                    RemarksMessage = "Trip " + updateTripStatusCommand.Status + " - Trip No. " + responseData.TripNo;
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

                //var addNotificationCommand = new AddNotificationCommand()
                //{
                //    TripId = updateTripStatusCommand.Id,
                //    TypeName = responseData.Name,
                //    SourceId = Guid.Parse(_userInfoToken.Id),
                //    Content = "Trip Status Changed By " + userResult.FirstName + " " + userResult.LastName,
                //    UserId = responseData.CreatedBy,
                //};
                //var notificationResult = await _mediator.Send(addNotificationCommand);

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

                        //if (userRoles.Count > 0)
                        //{
                        //    var travelDeskNotificationCommand = new AddNotificationCommand()
                        //    {
                        //        TripId = updateTripStatusCommand.Id,
                        //        TypeName = responseData.Name,
                        //        SourceId = Guid.Parse(_userInfoToken.Id),
                        //        Content = "Trip Status Changed By " + userResult.FirstName + " " + userResult.LastName,
                        //        UserId = userRoles.FirstOrDefault().UserId.Value,
                        //    };
                        //    var travelDeskNotificationResult = await _mediator.Send(travelDeskNotificationCommand);
                        //}
                    }
                }

                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];

                if (email == "Yes")
                {
                    if (updateTripStatusCommand.Status == "APPLIED" && updateTripStatusCommand.Approval != "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "Trip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();

                        var reportingHead = await _userRepository.FindAsync(responseData.CreatedByUser.ReportingTo.Value);

                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id)
                            .OrderByDescending(x => x.TripBy).OrderBy(x => x.DepartureDate).ToListAsync();

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
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(responseData.NoOfPerson == null ? "1" : responseData.NoOfPerson));

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + updateTripStatusCommand.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + updateTripStatusCommand.Id + "/" + responseData.CreatedBy);

                            string itineraryHtml = ItineraryHtml(itinerary, responseData.TripType);

                            if (hotel.Count > 0)
                            {
                                string itineraryHotelHtml = ItineraryHotelHtml(hotel, "Hotel");
                                itineraryHtml = itineraryHtml + itineraryHotelHtml;
                            }

                            templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

                            EmailHelper.SendEmail(new SendEmailSpecification
                            {
                                Body = templateBody,
                                FromAddress = defaultSmtp.UserName,
                                Host = defaultSmtp.Host,
                                IsEnableSSL = defaultSmtp.IsEnableSSL,
                                Password = defaultSmtp.Password,
                                Port = defaultSmtp.Port,
                                Subject = "Journey Requested - " + responseData.TripNo,
                                ToAddress = string.IsNullOrEmpty(responseData.CreatedByUser.AlternateEmail) ?
                                responseData.CreatedByUser.UserName :
                                responseData.CreatedByUser.UserName + "," + responseData.CreatedByUser.AlternateEmail,
                                CCAddress = TravelDesk == false ?
                                reportingHead.UserName :
                                reportingHead.UserName + ",travels@shyamsteel.com,bitan@shyamsteel.com",
                                UserName = defaultSmtp.UserName
                            });
                        }

                    }


                    if (updateTripStatusCommand.Approval == "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "Trip.html");
                        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                        var reportingHead = _userRepository.FindAsync(responseData.CreatedByUser.ReportingTo.Value).Result;

                        var itinerary = await _tripItineraryRepository.All.Where(x => x.TripId == updateTripStatusCommand.Id)
                            .OrderByDescending(x => x.TripBy).OrderBy(x => x.DepartureDate).ToListAsync();

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
                            templateBody = templateBody.Replace("{NO_OF_PERSON}", Convert.ToString(responseData.NoOfPerson == null ? "1" : responseData.NoOfPerson));

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + updateTripStatusCommand.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + updateTripStatusCommand.Id + "/" + responseData.CreatedBy);

                            string itineraryHtml = ItineraryHtml(itinerary, responseData.TripType);

                            if (hotel.Count > 0)
                            {
                                string itineraryHotelHtml = ItineraryHotelHtml(hotel, "Hotel");
                                itineraryHtml = itineraryHtml + itineraryHotelHtml;
                            }

                            templateBody = templateBody.Replace("{ITINERARY_HTML}", itineraryHtml);

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
                                Subject = "Journey Requested - " + responseData.TripNo,
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

                //*** Start Push Notification User ***
                MessageRequest userRequest = new MessageRequest()
                {
                    Body = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                    CustomKey = "Trip",
                    DeviceToken = responseData.CreatedByUser.DeviceKey,
                    DeviceType = responseData.CreatedByUser.IsDeviceTypeAndroid,
                    Id = responseData.Id.ToString(),
                    Title = "SFT Travel Desk",
                    UserId = responseData.CreatedBy.ToString()
                };
                var user = PushNotificationForTrip(userRequest);

                //*** End Push Notification User ***

                //*** Start Push Notification Reporting Head ***
                if (updateTripStatusCommand.Status == "APPLIED" && updateTripStatusCommand.Approval != "APPROVED")
                {
                    var reporting = await _userRepository.FindAsync(userResult.ReportingTo.Value);
                    MessageRequest rmRequest = new MessageRequest()
                    {
                        Body = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                        CustomKey = "Trip",
                        DeviceToken = reporting.DeviceKey,
                        DeviceType = reporting.IsDeviceTypeAndroid,
                        Id = responseData.Id.ToString(),
                        Title = "SFT Travel Desk",
                        UserId = responseData.CreatedBy.ToString()
                    };
                    var rmUser = PushNotificationForTrip(rmRequest);
                }

                if (updateTripStatusCommand.Approval == "APPROVED")
                {
                    var reporting = _userRepository.FindAsync(responseData.CreatedByUser.ReportingTo.Value).Result;

                    MessageRequest rmRequest = new MessageRequest()
                    {
                        Body = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                        CustomKey = "Trip",
                        DeviceToken = reporting.DeviceKey,
                        DeviceType = reporting.IsDeviceTypeAndroid,
                        Id = responseData.Id.ToString(),
                        Title = "SFT Travel Desk",
                        UserId = responseData.CreatedBy.ToString()
                    };
                    var rmUser = PushNotificationForTrip(rmRequest);
                }

                //*** End Push Notification Reporting Head ***

                //*** Start Notification User ***
                var addNotificationCommandUser = new AddNotificationCommand()
                {
                    TripId = responseData.Id,
                    TypeName = "Trip Status",
                    SourceId = responseData.CreatedByUser.Id,
                    Content = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                    UserId = responseData.CreatedByUser.Id,
                    Redirection = "/trip/details/" + responseData.Id
                };
                var notificationResultUser = await _mediator.Send(addNotificationCommandUser);
                //*** End Notification User ***


                //*** Start Notification Reporting Manager ***
                if (updateTripStatusCommand.Status == "APPLIED" && updateTripStatusCommand.Approval != "APPROVED")
                {
                    var reporting = await _userRepository.FindAsync(userResult.ReportingTo.Value);

                    var addNotificationCommandReportingManager = new AddNotificationCommand()
                    {
                        TripId = responseData.Id,
                        TypeName = "Trip Status",
                        SourceId = reporting.Id,
                        Content = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                        UserId = reporting.Id,
                        Redirection = "/trip/details/" + responseData.Id
                    };
                    var notificationResultReportingManager = await _mediator.Send(addNotificationCommandReportingManager);
                }
                if (updateTripStatusCommand.Approval == "APPROVED")
                {
                    var reporting = _userRepository.FindAsync(responseData.CreatedByUser.ReportingTo.Value).Result;

                    var addNotificationCommandReportingManager = new AddNotificationCommand()
                    {
                        TripId = responseData.Id,
                        TypeName = "Trip Status",
                        SourceId = reporting.Id,
                        Content = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                        UserId = reporting.Id,
                        Redirection = "/trip/details/" + responseData.Id
                    };
                    var notificationResultReportingManager = await _mediator.Send(addNotificationCommandReportingManager);
                }

                //*** End Notification Reporting Manager ***

                if (TravelDesk == true)
                {
                    //*** Start Notification Travel Desk & Bitan ***
                    var addNotificationCommandTravelDesk = new AddNotificationCommand()
                    {
                        TripId = responseData.Id,
                        TypeName = "Trip Status",
                        SourceId = Guid.Parse("D5DDC33E-BC3E-435F-AEE6-5D04F44A6565"),
                        Content = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                        UserId = Guid.Parse("D5DDC33E-BC3E-435F-AEE6-5D04F44A6565"),
                        Redirection = "/trip/details/" + responseData.Id
                    };
                    var notificationResultTravelDesk = await _mediator.Send(addNotificationCommandTravelDesk);

                    var addNotificationCommandBitan = new AddNotificationCommand()
                    {
                        TripId = responseData.Id,
                        TypeName = "Trip Status",
                        SourceId = Guid.Parse("D5DDC33E-BC3E-435F-AEE6-5D04F44A6565"),
                        Content = "Trip " + responseData.Status + " - Trip No. " + responseData.TripNo,
                        UserId = Guid.Parse("8DAACE8F-BBFC-46F1-94C8-F87AF34C8FDA"),
                        Redirection = "/trip/details/" + responseData.Id
                    };
                    var notificationResultBitan = await _mediator.Send(addNotificationCommandBitan);
                    //*** End Notification Travel Desk ***
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
                var responseData = await _tripRepository.FindAsync(updateStatusTripRequestAdvanceMoneyCommand.Id);
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = updateStatusTripRequestAdvanceMoneyCommand.Id,
                    TripTypeName = responseData.TripType,
                    ActionType = "Activity",
                    Remarks = updateStatusTripRequestAdvanceMoneyCommand.Status == string.Empty ?
                    "Requsted for advance money - Trip No." + responseData.TripNo
                    : "Requsted for advance money status updated",
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
                string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];

                if (email == "Yes")
                {
                    if (updateStatusTripRequestAdvanceMoneyCommand.Status == "APPROVED")
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoneyNewTemplate.html");
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
                            templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.AdvanceAccountApprovedAmount != null ? responseData.AdvanceAccountApprovedAmount : responseData.AdvanceMoneyApprovedAmount == null ? 0 : responseData.AdvanceMoneyApprovedAmount));
                            templateBody = templateBody.Replace("{REQUEST_MONEY}", Convert.ToString(responseData.AdvanceMoney));
                            templateBody = templateBody.Replace("{STATUS}", Convert.ToString(updateStatusTripRequestAdvanceMoneyCommand.Status));
                            templateBody = templateBody.Replace("{COLOUR}", updateStatusTripRequestAdvanceMoneyCommand.Status == "approved" ? Convert.ToString("#00ff1a") : Convert.ToString("#ff0000"));

                            var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                            templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                            templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + responseData.Id);
                            templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + responseData.Id + "/" + responseData.CreatedBy);

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

                        //*** Start Notification Reporting Manager ***
                        var addNotificationCommandReportingManager = new AddNotificationCommand()
                        {
                            TripId = responseData.Id,
                            TypeName = "Advance Money Approved",
                            SourceId = reportingHead.Id,
                            Content = "Advance Money Approved - Trip No. " + responseData.TripNo,
                            UserId = reportingHead.Id,
                            Redirection = "/trip/details/" + responseData.Id
                        };
                        var notificationResultReportingManager = await _mediator.Send(addNotificationCommandReportingManager);
                        //*** End Notification Reporting Manager ***

                        //*** Start Notification Accountant ***
                        foreach (var acc in accountant)
                        {
                            var addNotificationCommandAccountant = new AddNotificationCommand()
                            {
                                TripId = responseData.Id,
                                TypeName = "Advance Money Approved",
                                SourceId = acc.Id,
                                Content = "Advance Money Approved - Trip No. " + responseData.TripNo,
                                UserId = acc.Id,
                                Redirection = "/trip/details/" + responseData.Id
                            };
                            var notificationResultAccountant = await _mediator.Send(addNotificationCommandAccountant);
                            //*** End Notification Reporting Manager ***
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                var tripHotelBooking = await _tripItineraryRepository.FindAsync(addItineraryTicketBookingQuotationCommand.TripItineraryId);
                var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripTypeName = tripData.TripType,
                    ActionType = "Activity",
                    Remarks = "Itinerary ticket booking quotation added - Trip No. " + tripData.TripNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);


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

                    //*** Start Notification Created By User ***
                    var addNotificationCommandUser = new AddNotificationCommand()
                    {
                        TripId = tripDetails.Id,
                        TypeName = "Quotation Upload",
                        SourceId = tripDetails.CreatedByUser.Id,
                        Content = "Quotation Uploaded - Trip No. " + tripDetails.TripNo,
                        UserId = tripDetails.CreatedByUser.Id,
                        Redirection = "/trip/details/" + tripDetails.Id
                    };
                    var notificationResultUser = await _mediator.Send(addNotificationCommandUser);
                    //*** End Notification Created By User ***


                    //*** Start Notification Reporting Manager ***
                    var addNotificationCommandReportingManager = new AddNotificationCommand()
                    {
                        TripId = tripDetails.Id,
                        TypeName = "Quotation Upload",
                        SourceId = reportingHead.Id,
                        Content = "Quotation Uploaded - Trip No. " + tripDetails.TripNo,
                        UserId = reportingHead.Id,
                        Redirection = "/trip/details/" + tripDetails.Id
                    };
                    var notificationResultReportingManager = await _mediator.Send(addNotificationCommandReportingManager);
                    //*** End Notification Reporting Manager ***
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
                //var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                //var responseData = await _itineraryHotelBookingQuotationRepository.FindAsync(updateItineraryTicketBookingQuotationCommand.Id);
                //var tripHotelBooking = await _tripHotelBookingRepository.FindAsync(responseData.TripHotelBookingId);
                //var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = tripData.Id,
                //    TripTypeName = tripData.TripType,
                //    ActionType = "Activity",
                //    Remarks = "Itinerary ticket booking quotation has been updated for Trip No. " + tripData.TripNo,
                //    Status = "Updated",
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
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
            if (result.Success)
            {
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var responseData = await _itineraryHotelBookingQuotationRepository.FindAsync(Id);
                var tripHotelBooking = await _tripHotelBookingRepository.FindAsync(responseData.TripHotelBookingId);
                var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripTypeName = tripData.TripType,
                    ActionType = "Activity",
                    Remarks = "Itinerary ticket booking quotation deleted - Trip No. " + tripData.TripNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }
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
                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var tripHotelBooking = await _tripHotelBookingRepository.FindAsync(addItineraryHotelBookingQuotationCommand.TripHotelBookingId);
                var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripTypeName = tripData.TripType,
                    ActionType = "Activity",
                    Remarks = "Itinerary hotel booking quotation uploaded - Trip No. " + tripData.TripNo,
                    Status = "Added",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
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
                //var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                //var responseData = await _itineraryHotelBookingQuotationRepository.FindAsync(updateItineraryHotelBookingQuotationCommand.Id);
                //var tripHotelBooking = await _tripHotelBookingRepository.FindAsync(responseData.TripHotelBookingId);
                //var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                //var addTripTrackingCommand = new AddTripTrackingCommand()
                //{
                //    TripId = tripData.Id,
                //    TripTypeName = tripData.TripType,
                //    ActionType = "Activity",
                //    Remarks = "Itinerary hotel booking quotation has been updated for Trip No. " + tripData.TripNo,
                //    Status = "Updated",
                //    ActionBy = Guid.Parse(_userInfoToken.Id),
                //    ActionDate = DateTime.Now
                //};
                //var response = await _mediator.Send(addTripTrackingCommand);
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

            if (result.Success)
            {
                var responseData = await _itineraryHotelBookingQuotationRepository.FindAsync(Id);
                var tripHotelBooking = await _tripHotelBookingRepository.FindAsync(responseData.TripHotelBookingId);
                var tripData = await _tripRepository.FindAsync(tripHotelBooking.TripId);

                var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = tripData.Id,
                    TripTypeName = tripData.TripType,
                    ActionType = "Activity",
                    Remarks = "Itinerary hotel booking quotation deleted - Trip No. " + tripData.TripNo,
                    Status = "Deleted",
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var response = await _mediator.Send(addTripTrackingCommand);
            }

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
            BTTEM.Data.Entities.ResponseData response = new BTTEM.Data.Entities.ResponseData();
            UpdateApprovalTripRequestAdvanceMoneyCommand updateApprovalTripRequestAdvanceMoneyCommand = new UpdateApprovalTripRequestAdvanceMoneyCommand();
            foreach (var item in approvalTripRequestAdvanceMoneyCommand.UpdateApprovalTripRequestAdvanceMoneyCommand)
            {
                updateApprovalTripRequestAdvanceMoneyCommand = item;
                var result = await _mediator.Send(updateApprovalTripRequestAdvanceMoneyCommand);


                var trackingResponseData = await _tripRepository.FindAsync(item.Id);
                //var TrackingUserResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));
                var addTripTrackingCommand = new AddTripTrackingCommand()
                {
                    TripId = trackingResponseData.Id,
                    TripTypeName = trackingResponseData.TripType,
                    ActionType = "Activity",
                    Remarks = "Advance money " + item.AdvanceAccountApprovedStatus + " - Trip No." + trackingResponseData.TripNo,
                    Status = item.AdvanceAccountApprovedStatus,
                    ActionBy = Guid.Parse(_userInfoToken.Id),
                    ActionDate = DateTime.Now
                };
                var trackingResponse = await _mediator.Send(addTripTrackingCommand);


                //**Email Start**
                string email = this._configuration.GetSection("AppSettings")["Email"];
                string tripRedirectionURL = this._configuration.GetSection("TripRedirection")["TripRedirectionURL"];
                if (email == "Yes")
                {
                    var responseData = await _tripRepository.FindAsync(item.Id);
                    var userResult = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "AdvanceMoneyNewTemplate.html");
                    var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
                    var MoneyRequestBy = await _userRepository.FindAsync(responseData.CreatedBy);


                    List<User> accountant = new List<User>();
                    string toAccount = string.Empty;
                    if (responseData.CompanyAccountId == new Guid("d0ccea5f-5393-4a34-9df6-43a9f51f9f91"))
                    {
                        if (responseData.ProjectType == "Ongoing")
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
                        //var accounts = this._configuration.GetSection(responseData.AccountTeam)["UserEmail"];


                        var accountTeam = _companyAccountRepository.All.Where(x => x.Id == MoneyRequestBy.CompanyAccountId).FirstOrDefault().AccountTeam;

                        toAccount = this._configuration.GetSection(accountTeam)["UserEmail"];

                        // accountant = await _userRepository.All.Include(u => u.UserRoles)
                        // .Where(x => x.CompanyAccountId == MoneyRequestBy.CompanyAccountId).ToListAsync();

                        // accountant =
                        // accountant.Where(c => c.UserRoles.Select(cs => cs.RoleId)
                        //.Contains(new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2"))).ToList();
                    }

                    //if (string.IsNullOrEmpty(toAccount))
                    //{
                    //    toAccount = string.Join(',', accountant.Select(x => x.UserName));
                    //}

                    var reportingHead = await _userRepository.FindAsync(MoneyRequestBy.ReportingTo.Value);

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string templateBody = sr.ReadToEnd();
                        templateBody = templateBody.Replace("{NAME}", string.Concat(MoneyRequestBy.FirstName, " ", MoneyRequestBy.LastName));
                        templateBody = templateBody.Replace("{DATETIME}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                        templateBody = templateBody.Replace("{TRIP_NO}", Convert.ToString(responseData.TripNo));
                        templateBody = templateBody.Replace("{SOURCE_CITY}", Convert.ToString(responseData.SourceCityName));
                        templateBody = templateBody.Replace("{DESTINATION}", Convert.ToString(responseData.DestinationCityName));
                        templateBody = templateBody.Replace("{ADVANCE_MONEY}", Convert.ToString(responseData.AdvanceAccountApprovedAmount != null ? responseData.AdvanceAccountApprovedAmount : responseData.AdvanceMoneyApprovedAmount == null ? 0 : responseData.AdvanceMoneyApprovedAmount));
                        templateBody = templateBody.Replace("{REQUEST_MONEY}", Convert.ToString(responseData.AdvanceMoney));
                        templateBody = templateBody.Replace("{STATUS}", string.IsNullOrWhiteSpace(item.AdvanceAccountApprovedStatus) ? item.RequestAdvanceMoneyStatus : item.AdvanceAccountApprovedStatus);
                        templateBody = templateBody.Replace("{COLOUR}", item.AdvanceAccountApprovedStatus == "approved" ? Convert.ToString("#00ff1a") : Convert.ToString("#ff0000"));

                        var ca = await _companyAccountRepository.FindAsync(responseData.CompanyAccountId.Value);
                        templateBody = templateBody.Replace("{BILLING_COMPANY}", ca.AccountName);

                        templateBody = templateBody.Replace("{WEB_URL}", tripRedirectionURL + responseData.Id);
                        templateBody = templateBody.Replace("{APP_URL}", tripRedirectionURL + responseData.Id + "/" + responseData.CreatedBy);

                        EmailHelper.SendEmail(new SendEmailSpecification
                        {
                            Body = templateBody,
                            FromAddress = defaultSmtp.UserName,
                            Host = defaultSmtp.Host,
                            IsEnableSSL = defaultSmtp.IsEnableSSL,
                            Password = defaultSmtp.Password,
                            Port = defaultSmtp.Port,
                            Subject = "ADVANCE MONEY APPROVAL",
                            ToAddress = toAccount,
                            CCAddress = MoneyRequestBy.UserName + "," + reportingHead.UserName,
                            UserName = defaultSmtp.UserName
                        });
                    }

                    //*** Start Notification Reporting Manager ***
                    var addNotificationCommandReportingManager = new AddNotificationCommand()
                    {
                        TripId = responseData.Id,
                        TypeName = "Advance Money Approval",
                        SourceId = reportingHead.Id,
                        Content = "Advance money " + item.AdvanceAccountApprovedStatus + " - Trip No." + trackingResponseData.TripNo,
                        UserId = reportingHead.Id,
                        Redirection = "/trip/details/" + responseData.Id
                    };
                    var notificationResultReportingManager = await _mediator.Send(addNotificationCommandReportingManager);
                    //*** End Notification Reporting Manager ***


                    //*** Start Notification Created By User ***
                    var addNotificationCommandUser = new AddNotificationCommand()
                    {
                        TripId = responseData.Id,
                        TypeName = "Advance Money Approval",
                        SourceId = MoneyRequestBy.Id,
                        Content = "Advance money " + item.AdvanceAccountApprovedStatus + " - Trip No." + trackingResponseData.TripNo,
                        UserId = MoneyRequestBy.Id,
                        Redirection = "/trip/details/" + responseData.Id
                    };
                    var notificationResultUser = await _mediator.Send(addNotificationCommandUser);
                    //*** End Notification Created By User ***


                    //*** Start Notification Accountant ***
                    foreach (var acc in accountant)
                    {
                        var addNotificationCommandAccountant = new AddNotificationCommand()
                        {
                            TripId = responseData.Id,
                            TypeName = "Advance Money Approval",
                            SourceId = acc.Id,
                            Content = "Advance money " + item.AdvanceAccountApprovedStatus + " - Trip No." + trackingResponseData.TripNo,
                            UserId = acc.Id,
                            Redirection = "/trip/details/" + responseData.Id
                        };
                        var notificationResultAccountant = await _mediator.Send(addNotificationCommandAccountant);
                    }
                    //*** End Notification Accountant ***
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

        [NonAction]
        public string ItineraryHtml(List<TripItinerary> tripItineraries, string TripType)
        {
            string baseUrl = this._configuration.GetSection("Url")["BaseUrl"];
            StringBuilder sb = new StringBuilder();
            foreach (var item in tripItineraries)
            {
                sb.Append("<table class='journeyTableTop' style = 'background-color:#fff; box-shadow: -1px 4px 3px 2px #0000ff1a;'>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<div class='Journey startJourny'>");
                sb.Append("<p>Start Journey</p>");
                sb.Append("<h5>" + item.DepartureCityName + "</h5>");
                sb.Append("<h6><span>" + item.DepartureDate + "</span></h6>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<div class='journeyDetail'>");
                sb.Append("<p>" + item.BookTypeBy + "</p>");
                sb.Append("<div class='journeyImage'>");
                if (item.TripBy == "Bus")
                {
                    sb.Append("<img src = '" + baseUrl + "images/busImg.png' class='busImg' alt='' style='max-width: 27px; margin: 0 auto !important; display: block;'> ");
                }
                if (item.TripBy == "Flight")
                {
                    sb.Append("<img src = '" + baseUrl + "images/flightImg.png' class='busImg' alt=''>");
                }
                if (item.TripBy == "Train")
                {
                    sb.Append("<img src = '" + baseUrl + "images/trainImg2.png' class='busImg' alt=''>");
                }
                if (item.TripBy == "Car")
                {
                    sb.Append("<img src = '" + baseUrl + "images/carImg.png' class='busImg' alt=''>");
                }
                if (item.TripBy == "Hotel")
                {
                    sb.Append("<img src = '" + baseUrl + "images/hotelImg.png' class='busImg' alt=''>");
                }
                sb.Append("<img src = '" + baseUrl + "images/lines.png' class='lines' alt=''>");
                sb.Append("</div>");
                if (item.TripBy == "Bus")
                {
                    sb.Append("<p>Bus Booking</p>");
                }
                if (item.TripBy == "Flight")
                {
                    sb.Append("<p>Flight Booking</p>");
                }
                if (item.TripBy == "Train")
                {
                    sb.Append("<p>Train Booking</p>");
                }
                if (item.TripBy == "Car")
                {
                    sb.Append("<p>Car Booking</p>");
                }
                if (item.TripBy == "Hotel")
                {
                    sb.Append("<p>Hotel Booking</p>");
                }
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<div class='Journey endtJourny'>");
                sb.Append("<p>End Journey</p>");
                sb.Append("<h5>" + item.ArrivalCityName + "</h5>");
                sb.Append("<h6><span>" + item.DepartureDate + "</span></h6>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        [NonAction]
        public string ItineraryHotelHtml(List<TripHotelBooking> tripItinerariesHotel, string TripType)
        {
            string baseUrl = this._configuration.GetSection("Url")["BaseUrl"];
            StringBuilder sb = new StringBuilder();
            foreach (var item in tripItinerariesHotel)
            {
                sb.Append("<table class='journeyTableTop' style = 'background-color:#fff; box-shadow: -1px 4px 3px 2px #0000ff1a;'>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<div class='Journey startJourny'>");
                sb.Append("<p>Check-in</p>");
                sb.Append("<h5>" + item.CityName + "</h5>");
                sb.Append("<h6><span>" + item.CheckIn + "</span></h6>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<div class='journeyDetail'>");
                sb.Append("<p>" + item.BookTypeBy + "</p>");
                sb.Append("<div class='journeyImage'>");

                sb.Append("<img src = '" + baseUrl + "images/hotelImg.png' class='busImg' alt=''>");

                sb.Append("<img src = '" + baseUrl + "images/lines.png' class='lines' alt=''>");
                sb.Append("</div>");
                sb.Append("<p>Hotel Booking</p>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<div class='Journey endtJourny'>");
                sb.Append("<p>Check-out</p>");
                sb.Append("<h5>" + item.NearbyLocation + "</h5>");
                sb.Append("<h6><span>" + item.CheckOut + "</span></h6>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        [NonAction]
        public async Task<ResponseModel> PushNotificationForTrip(MessageRequest request)
        {
            NotificationModel message = null;
            if (!string.IsNullOrEmpty(request.DeviceToken) && request.DeviceType == false)
            {
                message = new NotificationModel()
                {
                    Message = new BTTEM.API.Models.Message
                    {
                        Token = request.DeviceToken,

                        Notification = new BTTEM.API.Models.Notification()
                        {
                            Title = request.Title,
                            Body = request.Body
                        },

                        Data = new BTTEM.API.Models.Data
                        {
                            NotificationTitle = request.Title,
                            NotificationBody = request.Body,
                            UserId = request.UserId,
                            Screen = "Profile",
                            CustomKey = request.CustomKey,
                            Id = request.Id
                        },

                        Android = new Android()
                        {
                            Priority = "high"
                        }
                    }
                };
            }
            else if (!string.IsNullOrEmpty(request.DeviceToken) && request.DeviceType == true)
            {
                message = new NotificationModel()
                {
                    Message = new BTTEM.API.Models.Message
                    {
                        Token = request.DeviceToken,

                        Data = new BTTEM.API.Models.Data
                        {
                            NotificationTitle = request.Title,
                            NotificationBody = request.Body,
                            UserId = request.UserId,
                            Screen = "Profile",
                            CustomKey = request.CustomKey,
                            Id = request.Id
                        },

                        Android = new Android()
                        {
                            Priority = "high"
                        }
                    }
                };
            }

            var resultNotification = await _notificationService.SendNotification(message);

            if (resultNotification.IsSuccess)
            {
                return resultNotification;
            }
            else
            {
                return resultNotification;
            }
        }
    }
}

