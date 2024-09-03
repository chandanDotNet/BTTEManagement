using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.Trip.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class CancelTripItineraryHotelCommandHandler : IRequestHandler<CancelTripItineraryHotelCommand, ServiceResponse<bool>>
    {

        private readonly ITripItineraryRepository _tripItineraryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelTripItineraryHotelCommandHandler> _logger;
        private readonly ITripHotelBookingRepository _tripHotelBookingRepository;
        private readonly ICancelTripItineraryHotelUserRepository _cancelTripItineraryHotelUserRepository;

        public CancelTripItineraryHotelCommandHandler(
           ITripItineraryRepository tripItineraryRepository,
           IUnitOfWork<POSDbContext> uow,
           IMapper mapper,
           ILogger<CancelTripItineraryHotelCommandHandler> logger,
           ITripHotelBookingRepository tripHotelBookingRepository,
           ICancelTripItineraryHotelUserRepository cancelTripItineraryHotelUserRepository
          )
        {
            _tripItineraryRepository = tripItineraryRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _tripHotelBookingRepository = tripHotelBookingRepository;
            _cancelTripItineraryHotelUserRepository = cancelTripItineraryHotelUserRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(CancelTripItineraryHotelCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.cancelTripItineraryHotel)
            {
                if (tv.IsItinerary == true)
                {
                    if (tv.Type == "REQUEST")
                    {

                        var entityExist = _tripItineraryRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                        if (entityExist != null)
                        {
                            entityExist.ApprovalStatus = "CANCEL REQUEST";
                            //if (!string.IsNullOrWhiteSpace(tv.NoOfTickets))
                            //{
                            //    entityExist.NoOfTickets = tv.NoOfTickets;
                            //}

                        }

                        _tripItineraryRepository.Update(entityExist);

                        if (tv.cancelTripItineraryHotelUsers != null)
                        {
                            var groupTripExist = _cancelTripItineraryHotelUserRepository.All.Where(v => v.TripItineraryId == tv.Id).ToList();
                            if (groupTripExist.Count > 0)
                            {
                                _cancelTripItineraryHotelUserRepository.RemoveRange(groupTripExist);
                            }

                            tv.cancelTripItineraryHotelUsers.ForEach(item =>
                            {
                                //item.TripId = request.TripId;
                                item.Id = Guid.NewGuid();
                            });

                            var groupTrip = _mapper.Map<List<CancelTripItineraryHotelUser>>(tv.cancelTripItineraryHotelUsers);
                            _cancelTripItineraryHotelUserRepository.AddRange(groupTrip);
                        }
                    }
                    if (tv.Type == "APPROVE")
                    {

                        var groupTripExist = _cancelTripItineraryHotelUserRepository.All.Where(v => v.TripItineraryId == tv.Id && v.IsCancelrequest==false).ToList();
                        
                        var entityExist = _tripItineraryRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                        if (entityExist != null)
                        {
                            entityExist.ApprovalStatus = "CANCEL APPROVED";
                            entityExist.NoOfTickets = groupTripExist.Count.ToString();
                            

                        }

                        _tripItineraryRepository.Update(entityExist);
                    }
                    if (tv.Type == "REJECT")
                    {

                        var groupTripExist = _cancelTripItineraryHotelUserRepository.All.Where(v => v.TripItineraryId == tv.Id && v.IsCancelrequest == false).ToList();

                        var entityExist = _tripItineraryRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                        if (entityExist != null)
                        {
                            entityExist.ApprovalStatus = "CANCEL REJECTED";
                            //if (!string.IsNullOrWhiteSpace(tv.NoOfTickets))
                            //{
                            //    entityExist.NoOfTickets = groupTripExist.Count.ToString();
                            //}

                        }

                        _tripItineraryRepository.Update(entityExist);
                    }


                }
                else
                {
                    if (tv.Type == "REQUEST")
                    {
                        var entityExist = _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                        if (entityExist != null)
                        {
                            entityExist.ApprovalStatus = "CANCEL REQUEST";
                            //if (!string.IsNullOrWhiteSpace(tv.NoOfRoom))
                            //{
                            //    entityExist.NoOfRoom = tv.NoOfRoom;
                            //}

                        }

                        _tripHotelBookingRepository.Update(entityExist);

                        if (tv.cancelTripItineraryHotelUsers != null)
                        {
                            var groupTripExist = _cancelTripItineraryHotelUserRepository.All.Where(v => v.TripItineraryId == entityExist.Id).ToList();
                            if (groupTripExist.Count > 0)
                            {
                                _cancelTripItineraryHotelUserRepository.RemoveRange(groupTripExist);
                            }

                            tv.cancelTripItineraryHotelUsers.ForEach(item =>
                            {
                                //item.TripId = request.TripId;
                                item.Id = Guid.NewGuid(); 
                                item.IsHotel = true;
                            });

                            var groupTrip = _mapper.Map<List<CancelTripItineraryHotelUser>>(tv.cancelTripItineraryHotelUsers);
                            _cancelTripItineraryHotelUserRepository.AddRange(groupTrip);
                        }

                    }
                    if (tv.Type == "APPROVE")
                    {

                        var entityExist = _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                        if (entityExist != null)
                        {
                            entityExist.ApprovalStatus = "CANCEL APPROVED";
                            //if (!string.IsNullOrWhiteSpace(tv.NoOfRoom))
                            //{ 
                            //    entityExist.NoOfRoom = tv.NoOfRoom; 
                            //}

                        }

                        _tripHotelBookingRepository.Update(entityExist);
                    }
                    if (tv.Type == "REJECT")
                    {

                        var entityExist = _tripHotelBookingRepository.FindBy(v => v.Id == tv.Id).FirstOrDefault();
                        if (entityExist != null)
                        {
                            entityExist.ApprovalStatus = "CANCEL REJECTED";
                            //if (!string.IsNullOrWhiteSpace(tv.NoOfRoom))
                            //{
                            //    entityExist.NoOfRoom = tv.NoOfRoom;
                            //}

                        }

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
