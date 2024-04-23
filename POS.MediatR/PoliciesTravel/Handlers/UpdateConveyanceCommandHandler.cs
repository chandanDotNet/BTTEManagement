using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class UpdateConveyanceCommandHandler : IRequestHandler<UpdateConveyanceCommand, ServiceResponse<bool>>
    {


        private readonly IConveyanceRepository _conveyanceRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdateConveyanceCommandHandler> _logger;
        public UpdateConveyanceCommandHandler(
           IConveyanceRepository conveyanceRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdateConveyanceCommandHandler> logger
            )
        {
            _conveyanceRepository = conveyanceRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }


        public async Task<ServiceResponse<bool>> Handle(UpdateConveyanceCommand request, CancellationToken cancellationToken)
        {

            foreach (var tv in request.conveyancesData)
            {
                var travelsModeUpdate = _mapper.Map<Data.Conveyance>(tv);

                var travelsModeExit = await _conveyanceRepository.AllIncluding(c => c.conveyancesItem).Where(a => a.Id == tv.Id).FirstOrDefaultAsync();

                travelsModeExit.Name = travelsModeUpdate.Name;
                travelsModeExit.IsMaster = travelsModeUpdate.IsMaster;


                travelsModeExit.conveyancesItem.ForEach(c =>
                {
                    // var travelsModeExit2 =  _travelsModeRepository.AllIncluding(c => c.classOfTravels).Where(c => c.Id == c.Id).FirstOrDefaultAsync();
                    c.ConveyanceId = travelsModeUpdate.Id;
                    //c.ClassName = travelsModeUpdate.classOfTravels.Where(Id == c.Id);
                    c.IsCheck = travelsModeUpdate.conveyancesItem.Where(f => f.Id == c.Id).Select(n => n.IsCheck).FirstOrDefault();
                    c.Amount = travelsModeUpdate.conveyancesItem.Where(f => f.Id == c.Id).Select(n => n.Amount).FirstOrDefault();
                });
                //foreach (var policy in travelsModeUpdate.classOfTravels)
                //{

                //}
                _conveyanceRepository.Update(travelsModeExit);

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
