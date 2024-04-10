using AutoMapper;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.SalesOrder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class UpdateTravelModeCommandHandler : IRequestHandler<UpdateTravelModeCommand, ServiceResponse<bool>>
    {
        private readonly ITravelsModeRepository _travelsModeRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdateTravelModeCommandHandler> _logger;
        public UpdateTravelModeCommandHandler(
           ITravelsModeRepository travelsModeRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdateTravelModeCommandHandler> logger
            )
        {
            _travelsModeRepository = travelsModeRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdateTravelModeCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.TravelModeData)
            {
                var travelsModeUpdate = _mapper.Map<Data.TravelMode>(tv);

                var travelsModeExit = await _travelsModeRepository.AllIncluding(c => c.classOfTravels).Where(a=>a.Id==tv.Id).FirstOrDefaultAsync();

                travelsModeExit.TravelsModesName = travelsModeUpdate.TravelsModesName;
                travelsModeExit.IsMaster = travelsModeUpdate.IsMaster;
                travelsModeExit.PoliciesDetailId = travelsModeUpdate.PoliciesDetailId;
                travelsModeExit.IsCheck = travelsModeUpdate.IsCheck;

                travelsModeExit.classOfTravels.ForEach(c =>
                {
                   // var travelsModeExit2 =  _travelsModeRepository.AllIncluding(c => c.classOfTravels).Where(c => c.Id == c.Id).FirstOrDefaultAsync();
                    c.TravelModeId = travelsModeUpdate.Id;
                    //c.ClassName = travelsModeUpdate.classOfTravels.Where(Id == c.Id);
                   c.IsCheck = travelsModeUpdate.classOfTravels.Where(f => f.Id == c.Id).Select(n => n.IsCheck).FirstOrDefault();
                });
                //foreach (var policy in travelsModeUpdate.classOfTravels)
                //{

                //}
                _travelsModeRepository.Update(travelsModeExit);

            }
            
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Updating travels Mode.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith201(true);
        }

    }
}
