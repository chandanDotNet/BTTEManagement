using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PathHelper = POS.Helper.PathHelper;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class UpdateTripHotelBookingCommandHandler : IRequestHandler<UpdateTripHotelBookingCommand, ServiceResponse<bool>>
    {

        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTripHotelBookingCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IUserRepository _userRepository;
        private readonly UserInfoToken _userInfoToken;

        public UpdateTripHotelBookingCommandHandler(
           ITripHotelBookingRepository tripHotelBookingRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<UpdateTripHotelBookingCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            IUserRepository userRepository,
           UserInfoToken userInfoToken
          )
        {
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _userRepository = userRepository;
            _userInfoToken = userInfoToken;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateTripHotelBookingCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _userRepository.FindAsync(Guid.Parse(_userInfoToken.Id));

            foreach (var tv in request.tripHotelBooking)
            {
                if (tv.Id == Guid.Empty)
                {
                    var entity = _mapper.Map<Data.TripHotelBooking>(tv);
                    entity.Id = Guid.NewGuid();
                    entity.ApprovalStatus = "PENDING";

                    if (userDetails.IsDirector)
                    {
                        entity.ApprovalStatus = "APPROVED";
                        entity.ApprovalStatusDate = DateTime.Now;
                    }

                    _tripHotelBookingRepository.Add(entity);
                }
                else
                {
                    var entityExist = await _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefaultAsync();
                    if (entityExist != null)
                    {
                        if (tv.TripId.HasValue)
                        {
                            entityExist.TripId = (Guid)tv.TripId;
                        }
                        if (tv.CheckIn.HasValue)
                        {
                            entityExist.CheckIn = (DateTime)tv.CheckIn;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.BookTypeBy))
                        {
                            entityExist.BookTypeBy = tv.BookTypeBy;
                        }
                        if (tv.CheckOut.HasValue)
                        {
                            entityExist.CheckOut = (DateTime)tv.CheckOut;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CheckInTime))
                        {
                            entityExist.CheckInTime = tv.CheckInTime;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CheckOutTime))
                        {
                            entityExist.CheckOutTime = tv.CheckOutTime;
                        }
                        if (tv.CityId.HasValue)
                        {
                            entityExist.CityId = tv.CityId;
                        }
                        if (tv.TentativeAmount > 0)
                        {
                            entityExist.TentativeAmount = tv.TentativeAmount;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CityName))
                        {
                            entityExist.CityName = tv.CityName;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.NearbyLocation))
                        {
                            entityExist.NearbyLocation = tv.NearbyLocation;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.PreferredHotel))
                        {
                            entityExist.PreferredHotel = tv.PreferredHotel;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.BookStatus))
                        {
                            entityExist.BookStatus = tv.BookStatus;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.ApprovalStatus))
                        {
                            if (userDetails.IsDirector)
                            {
                                entityExist.ApprovalStatus = "APPROVED";
                                entityExist.ApprovalStatusDate = DateTime.Now;
                            }
                            else
                            {
                                entityExist.ApprovalStatus = tv.ApprovalStatus;
                            }                            
                        }
                        if (tv.CancelationCharge > 0)
                        {
                            entityExist.CancelationCharge = tv.CancelationCharge;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.CancelationReason))
                        {
                            entityExist.CancelationReason = tv.CancelationReason;
                        }
                        if (tv.CancelationDate.HasValue)
                        {
                            entityExist.CancelationDate = tv.CancelationDate;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.NoOfRoom))
                        {
                            entityExist.NoOfRoom = tv.NoOfRoom;
                        }
                        entityExist.IsReschedule = tv.IsReschedule;
                        if (!string.IsNullOrWhiteSpace(tv.RescheduleStatus))
                        {
                            entityExist.RescheduleStatus = tv.RescheduleStatus;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.RescheduleReason))
                        {
                            entityExist.RescheduleReason = tv.RescheduleReason;
                        }
                        if (tv.RescheduleCharge > 0)
                        {
                            entityExist.RescheduleCharge = tv.RescheduleCharge;
                        }
                        if (tv.RescheduleCheckIn.HasValue)
                        {
                            entityExist.RescheduleCheckIn = (DateTime)tv.RescheduleCheckIn;
                        }
                        if (tv.RescheduleCheckOut.HasValue)
                        {
                            entityExist.RescheduleCheckOut = (DateTime)tv.RescheduleCheckOut;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.RescheduleCheckInTime))
                        {
                            entityExist.RescheduleCheckInTime = tv.RescheduleCheckInTime;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.RescheduleCheckOutTime))
                        {
                            entityExist.RescheduleCheckOutTime = tv.RescheduleCheckOutTime;
                        }
                        if (tv.VendorId.HasValue)
                        {
                            entityExist.VendorId = (Guid)tv.VendorId;
                        }
                        if (tv.AgentCharge > 0)
                        {
                            entityExist.AgentCharge = tv.AgentCharge;
                        }
                        if (tv.BookingAmount > 0)
                        {
                            entityExist.BookingAmount = tv.BookingAmount;
                        }
                        if (!string.IsNullOrWhiteSpace(tv.BookingNumber))
                        {
                            entityExist.BookingNumber = tv.BookingNumber;
                        }                       

                        if (tv.IsRescheduleChargePlus == true)
                        {
                            entityExist.TotalAmount = (tv.AgentCharge == null ? entityExist.AgentCharge : tv.AgentCharge)
                                                     + (tv.BookingAmount == null ? entityExist.BookingAmount : tv.BookingAmount)
                                                     - (tv.CancelationCharge == null ? entityExist.CancelationCharge : tv.CancelationCharge)
                                                     + (tv.RescheduleCharge == null ? entityExist.RescheduleCharge : tv.RescheduleCharge);
                        }
                        else
                        {
                            entityExist.TotalAmount = (tv.AgentCharge == null ? entityExist.AgentCharge : tv.AgentCharge)
                                                    + (tv.BookingAmount == null ? entityExist.BookingAmount : tv.BookingAmount)
                                                    - (tv.CancelationCharge == null ? entityExist.CancelationCharge : tv.CancelationCharge)
                                                    - (tv.RescheduleCharge == null ? entityExist.RescheduleCharge : tv.RescheduleCharge);
                        }

                        //==================  Ticket Upload

                        if (!string.IsNullOrWhiteSpace(tv.RescheduleBillReceiptName) && !string.IsNullOrWhiteSpace(tv.RescheduleBillReceiptPath))
                        {
                            string contentRootPath = _webHostEnvironment.WebRootPath;
                            var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                            if (!Directory.Exists(pathToSave))
                            {
                                Directory.CreateDirectory(pathToSave);
                            }

                            var extension = Path.GetExtension(tv.RescheduleBillReceiptName);
                            var id = Guid.NewGuid();
                            var path = $"{id}.{extension}";
                            var documentPath = Path.Combine(pathToSave, path);
                            string base64 = tv.RescheduleBillReceiptPath.Split(',').LastOrDefault();
                            if (!string.IsNullOrWhiteSpace(base64))
                            {
                                byte[] bytes = Convert.FromBase64String(base64);
                                try
                                {
                                    await File.WriteAllBytesAsync($"{documentPath}", bytes);
                                    entityExist.RescheduleBillReceiptPath = path;
                                    entityExist.RescheduleBillReceiptName = tv.RescheduleBillReceiptName;
                                }
                                catch
                                {
                                    _logger.LogError("Error while saving files", entityExist);
                                }
                            }
                        }

                        //==================  Approval Document Upload

                        if (!string.IsNullOrWhiteSpace(tv.RescheduleApprovalDocumentsReceiptName) && !string.IsNullOrWhiteSpace(tv.RescheduleApprovalDocumentsReceiptPath))
                        {
                            string contentRootPath = _webHostEnvironment.WebRootPath;
                            var pathToSave = Path.Combine(contentRootPath, _pathHelper.TravelDeskAttachments);

                            if (!Directory.Exists(pathToSave))
                            {
                                Directory.CreateDirectory(pathToSave);
                            }

                            var extension = Path.GetExtension(tv.RescheduleApprovalDocumentsReceiptName);
                            var id = Guid.NewGuid();
                            var path = $"{id}.{extension}";
                            var documentPath = Path.Combine(pathToSave, path);
                            string base64 = tv.RescheduleApprovalDocumentsReceiptPath.Split(',').LastOrDefault();
                            if (!string.IsNullOrWhiteSpace(base64))
                            {
                                byte[] bytes = Convert.FromBase64String(base64);
                                try
                                {
                                    await File.WriteAllBytesAsync($"{documentPath}", bytes);
                                    entityExist.RescheduleApprovalDocumentsReceiptPath = path;
                                    entityExist.RescheduleApprovalDocumentsReceiptName = tv.RescheduleApprovalDocumentsReceiptName;
                                }
                                catch
                                {
                                    _logger.LogError("Error while saving files", entityExist);
                                }
                            }
                        }

                        //=================

                        _tripHotelBookingRepository.Update(entityExist);
                    }
                }






            }


            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }

    }
}
