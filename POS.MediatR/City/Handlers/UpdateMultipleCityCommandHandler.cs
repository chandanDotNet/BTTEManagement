using AutoMapper;
using BTTEM.MediatR.City.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using POS.Helper;
using POS.MediatR.City.Commands;
using POS.MediatR.City.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.City.Handlers
{
    public class UpdateMultipleCityCommandHandler : IRequestHandler<UpdateMultipleCityCommand, ServiceResponse<bool>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMultipleCityCommandHandler> _logger;
        public UpdateMultipleCityCommandHandler(
           ICityRepository cityRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateMultipleCityCommandHandler> logger
            )
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(UpdateMultipleCityCommand request, CancellationToken cancellationToken)
        {
            //var entityExist = await _cityRepository
            //    .All.FirstOrDefaultAsync(c => c.CityName == request.CityName && c.CountryId == request.CountryId && c.Id != request.Id);

            //if (entityExist != null)
            //{
            //    _logger.LogError("City Already Exist.");
            //    return ServiceResponse<bool>.Return409("City Already Exist.");
            //}
            foreach (var item in request.Cities)           
            {
                var entityExist = await _cityRepository.FindBy(v => v.Id == item.Id).FirstOrDefaultAsync();
                entityExist.IsMetroCity = item.IsMetroCity;
                _cityRepository.Update(entityExist);
            }           

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}