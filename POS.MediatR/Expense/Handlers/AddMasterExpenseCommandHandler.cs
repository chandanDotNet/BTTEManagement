using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddMasterExpenseCommandHandler : IRequestHandler<AddMasterExpenseCommand, ServiceResponse<MasterExpenseDto>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserInfoToken _userInfoToken;

        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddMasterExpenseCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddMasterExpenseCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddMasterExpenseCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            UserInfoToken userInfoToken,
            IUserRoleRepository userRoleRepository)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
        }


        public async Task<ServiceResponse<MasterExpenseDto>> Handle(AddMasterExpenseCommand request, CancellationToken cancellationToken)
        {
            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
            var Role = GetUserRole(LoginUserId).Result.FirstOrDefault();

            if (request.TripId.HasValue && request.TripId.Value != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                var masterExpenseExist = await _masterExpenseRepository.All.Where(x => x.TripId == request.TripId.Value).ToListAsync();

                if (masterExpenseExist.Count > 0)
                {
                    for (int i = 0; i < masterExpenseExist.Count; i++)
                    {
                        var userRole = GetUserRole(masterExpenseExist[i].CreatedBy).Result.FirstOrDefault();

                        request.Id = masterExpenseExist[i].Id;
                        request.TripId = masterExpenseExist[i].TripId;

                        //if (Role.Id == userRole.Id) //Travel Desk 
                        //{
                        //    request.Id = masterExpenseExist[i].Id;
                        //    request.TripId = masterExpenseExist[i].TripId;
                        //    var requestEntity = _mapper.Map<MasterExpense>(request);
                        //    var mapEntity = _mapper.Map<MasterExpenseDto>(requestEntity);
                        //    return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(mapEntity);
                        //}

                        //if (Role.Id != userRole.Id) //Apart From Travel Desk
                        //{
                        //    request.Id = masterExpenseExist[i].Id;
                        //    request.TripId = masterExpenseExist[i].TripId;
                        //    var requestEntity = _mapper.Map<MasterExpenseDto>(request);
                        //    return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(requestEntity);
                        //}

                    }
                    return ServiceResponse<MasterExpenseDto>.Return409("Expense Already Exist For This Trip.");
                }
            }

            var entity = _mapper.Map<MasterExpense>(request);
            entity.Id = Guid.NewGuid();
            //entity.Status = "YET TO SUBMIT";
            entity.ApprovalStage = "PENDING";
            entity.ReimbursementStatus = "PENDING";
            entity.RollbackCount = 0;

            entity.GroupExpenses.ForEach(item =>
            {
                item.MasterExpenseId = entity.Id;
                item.Id = Guid.NewGuid();
            });

            _masterExpenseRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while saving Master Expense");
                return ServiceResponse<MasterExpenseDto>.Return500();
            }

            var industrydto = _mapper.Map<MasterExpenseDto>(entity);
            return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(industrydto);

            //var roleDetails = await _userRoleRepository.All.FirstOrDefaultAsync(x => x.UserId == request.);

            //var masterExpenseExist = await _masterExpenseRepository.All.Where(x => x.TripId == request.TripId.Value).ToListAsync();           

            //if (roleDetails.RoleId == new Guid("F72616BE-260B-41BB-A4EE-89146622179A"))
            //{
            //    request.Id = masterExpenseExist.Id;
            //    request.TripId = masterExpenseExist.TripId;
            //    var requestEntity = _mapper.Map<MasterExpenseDto>(request);
            //    return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(requestEntity);
            //}
            //if (masterExpenseExist != null)
            //{
            //    request.Id = masterExpenseExist.Id;
            //    request.TripId = masterExpenseExist.TripId;
            //    var requestEntity = _mapper.Map<MasterExpenseDto>(request);
            //    return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(requestEntity);
            //}

            //var entity = _mapper.Map<MasterExpense>(request);
            //entity.Id = Guid.NewGuid();

            //_masterExpenseRepository.Add(entity);

            //if (await _uow.SaveAsync() <= 0)
            //{
            //    _logger.LogError("Error while saving Master Expense");
            //    return ServiceResponse<MasterExpenseDto>.Return500();
            //}

            //var industrydto = _mapper.Map<MasterExpenseDto>(entity);
            //return ServiceResponse<MasterExpenseDto>.ReturnResultWith200(industrydto);

        }
        public async Task<List<RoleDto>> GetUserRole(Guid Id)
        {
            var rolesDetails = await _userRoleRepository.AllIncluding(c => c.Role).Where(d => d.UserId == Id)
                .ToListAsync();

            List<RoleDto> roleDto = new List<RoleDto>();
            foreach (var role in rolesDetails)
            {
                RoleDto rd = new RoleDto();
                rd.Id = role.Role.Id;
                rd.Name = role.Role.Name;
                roleDto.Add(rd);
            }
            // var roleClaims = await _roleClaimRepository.All.Where(c => rolesIds.Contains(c.RoleId)).Select(c => c.ClaimType).ToListAsync();
            return roleDto;
        }
    }
}
