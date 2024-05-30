using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.RequestACall.Handler;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
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

namespace BTTEM.MediatR.CompanyProfile.Handlers
{
    public class AddUpdateStateWiseGSTCommandHandler : IRequestHandler<AddUpdateStateWiseGSTCommand, ServiceResponse<List<CompanyGSTDto>>>
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ICompanyGSTRepository _stateWiseGSTReepository;
        private readonly ILogger<AddUpdateStateWiseGSTCommandHandler> _logger;
        public AddUpdateStateWiseGSTCommandHandler(IUnitOfWork<POSDbContext> uow,
            IMapper mapper, ICompanyGSTRepository stateWiseGSTReepository,
            ILogger<AddUpdateStateWiseGSTCommandHandler> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _stateWiseGSTReepository = stateWiseGSTReepository;
            _logger = logger;
        }
        public async Task<ServiceResponse<List<CompanyGSTDto>>> Handle(AddUpdateStateWiseGSTCommand request, CancellationToken cancellationToken)
        {
            request.StateWiseGSTList.ForEach(item =>
            {
                var entityExist = _stateWiseGSTReepository.All.Where(x => x.Id == item.Id).FirstOrDefault();
                if (entityExist != null)
                {
                    entityExist.CompanyAccountId = item.CompanyAccountId;
                    entityExist.StateId = item.StateId;
                    entityExist.GSTNo = item.GSTNo;
                    _stateWiseGSTReepository.Update(entityExist);
                }
                else
                {
                    var entity = _mapper.Map<CompanyGST>(item);
                    entity.Id = Guid.NewGuid();
                    _stateWiseGSTReepository.Add(entity);
                }
            });

            //var entity = _mapper.Map<List<CompanyGST>>(request.StateWiseGSTList);
            //entity.ForEach(item =>
            //{
            //    item.Id = Guid.NewGuid();
            //});
            //_stateWiseGSTReepository.AddRange(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving contact support");
                return ServiceResponse<List<CompanyGSTDto>>.Return500();
            }            
            return ServiceResponse<List<CompanyGSTDto>>.ReturnResultWith200(request.StateWiseGSTList);
        }
    }
}
