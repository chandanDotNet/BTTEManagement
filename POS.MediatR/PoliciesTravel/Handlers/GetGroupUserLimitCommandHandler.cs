using BTTEM.Data.Dto;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class GetGroupUserLimitCommandHandler : IRequestHandler<GetGroupUserLimitCommand, ServiceResponse<GroupUserLimitList>>
    {
        private readonly ILogger<GetGroupUserLimitCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IPoliciesDetailRepository _policiesDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPoliciesLodgingFoodingRepository _policiesLodgingFoodingRepository;
        public GetGroupUserLimitCommandHandler(
            ILogger<GetGroupUserLimitCommandHandler> logger,
            IMediator mediator,
            IUnitOfWork<POSDbContext> uow,
            IUserRepository userRepository,
            IPoliciesDetailRepository policiesDetailRepository,
            IPoliciesLodgingFoodingRepository policiesLodgingFoodingRepository
            )
        {
            _logger = logger;
            _mediator = mediator;
            _uow = uow;
            _userRepository = userRepository;
            _policiesDetailRepository = policiesDetailRepository;
            _policiesLodgingFoodingRepository = policiesLodgingFoodingRepository;
        }
        public async Task<ServiceResponse<GroupUserLimitList>> Handle(GetGroupUserLimitCommand request, CancellationToken cancellationToken)
        {
            List<GroupUserLimitList> groupUserLimitList = new List<GroupUserLimitList>();
            var users = await _userRepository.All.Where(x => request.UersIds.Contains(x.Id)).ToListAsync();
            Dictionary<Guid, string> queryResult = new Dictionary<Guid, string>();
            users.ForEach(item =>
            {
                var policyId = _policiesDetailRepository.All.Where(x => x.CompanyAccountId == item.CompanyAccountId && x.GradeId == item.GradeId).FirstOrDefault().Id;
                var policyName = _policiesDetailRepository.All.Where(x => x.CompanyAccountId == item.CompanyAccountId && x.GradeId == item.GradeId).FirstOrDefault().Name;
                queryResult.Add(policyId, policyName);
            });

            foreach (var item in queryResult)
            {
                var _mc = await _policiesLodgingFoodingRepository.All.FirstOrDefaultAsync(x => x.PoliciesDetailId == item.Key && x.IsMetroCities == true);
                var _oc = await _policiesLodgingFoodingRepository.All.FirstOrDefaultAsync(x => x.PoliciesDetailId == item.Key && x.OtherCities == true);
                var _foo = await _policiesLodgingFoodingRepository.All.FirstOrDefaultAsync(x => x.PoliciesDetailId == item.Key && x.IsBudget == true);

                groupUserLimitList.Add(new GroupUserLimitList
                {
                    MetroCity = _mc == null ? 0 : _mc.MetroCitiesUptoAmount,
                    IsMetroCity = _mc == null ? false : _mc.IsMetroCities,

                    OtherCity = _oc == null ? 0 : _oc.OtherCitiesUptoAmount,
                    IsOtherCity = _oc == null ? false : _oc.OtherCities,

                    Fooding = _foo == null ? 0 : _foo.BudgetAmount,
                    IsFooding = _foo == null ? false : _oc.IsBudget,

                    DA = _policiesDetailRepository.All.FirstOrDefault(x => x.Id == item.Key).DailyAllowance
                });
            };

            GroupUserLimitList result = new GroupUserLimitList()
            {
                MetroCity = groupUserLimitList.Where(x => x.IsMetroCity).Sum(x => x.MetroCity),
                IsMetroCity = groupUserLimitList.Any(x => x.IsMetroCity),
                OtherCity = groupUserLimitList.Where(x => x.IsOtherCity).Sum(x => x.OtherCity),
                IsOtherCity = groupUserLimitList.Any(x => x.IsOtherCity),
                Fooding = groupUserLimitList.Where(x => x.IsFooding).Sum(x => x.Fooding),
                IsFooding = groupUserLimitList.Any(x => x.IsFooding),
                DA = groupUserLimitList.Sum(x => x.DA),
            };
            return ServiceResponse<GroupUserLimitList>.ReturnResultWith200(result);
        }
    }
}
