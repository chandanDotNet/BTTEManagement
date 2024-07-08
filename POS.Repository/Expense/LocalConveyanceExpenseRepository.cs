using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.Repository.Expense;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class LocalConveyanceExpenseRepository : GenericRepository<LocalConveyanceExpense, POSDbContext>,
            ILocalConveyanceExpenseRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRoleRepository _userRoleRepository;
        public LocalConveyanceExpenseRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
             UserInfoToken userInfoToken,
              IUserRoleRepository userRoleRepository
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<LocalConveyanceExpenseList> GetAllLocalConveyanceExpense(LocalConveyanceExpenseResource expenseResource)
        {

            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
            var Role = GetUserRole(LoginUserId).Result.FirstOrDefault();

            
            if(expenseResource.IsReport==true)
            {
                expenseResource.CreatedBy = expenseResource.UserId;
            }
            else
            {
                if (expenseResource.IsMyRequest == true)
                {
                    expenseResource.CreatedBy = LoginUserId;
                }
                else
                {
                    if (Role != null)
                    {
                        if (Role.Id == new Guid("F9B4CCD2-6E06-443C-B964-23BF935F859E")) //Reporting Manager
                        {
                            expenseResource.ReportingHeadId = LoginUserId;
                        }
                        //else if (Role.Id == new Guid("F72616BE-260B-41BB-A4EE-89146622179A")) //Travel Desk
                        //{
                        //    tripResource.ReportingHeadId = null;
                        //}

                        else if (Role.Id == new Guid("E1BD3DCE-EECF-468D-B930-1875BD59D1F4")) //Submitter
                        {
                            expenseResource.CreatedBy = LoginUserId;
                        }
                    }
                }
            }

                

            

            var collectionBeforePaging = All.Include(t => t.CreatedByUser).Include(a=>a.Documents).ApplySort(expenseResource.OrderBy,
              _propertyMappingService.GetPropertyMapping<LocalConveyanceExpenseDto, LocalConveyanceExpense>());

            if (expenseResource.Id.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Id == expenseResource.Id);
            }
            if (expenseResource.CreatedBy.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedBy == expenseResource.CreatedBy);
            }
            if (expenseResource.ReportingHeadId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedByUser.ReportingTo == expenseResource.ReportingHeadId);
            }
            if (expenseResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseDate >= new DateTime(expenseResource.FromDate.Value.Year, expenseResource.FromDate.Value.Month, expenseResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (expenseResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseDate <= new DateTime(expenseResource.ToDate.Value.Year, expenseResource.ToDate.Value.Month, expenseResource.ToDate.Value.Day, 23, 59, 59));
            }
            if (!string.IsNullOrEmpty(expenseResource.SearchQuery))
            {
                var searchQueryForWhereClause = expenseResource.SearchQuery
              .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                    EF.Functions.Like(a.Status, $"%{searchQueryForWhereClause}%") 
                    || EF.Functions.Like(a.From, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.To, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.ExpenseDate.ToString(), $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Status, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.ApproxKM.ToString(), $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Amount.ToString(), $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Particular, $"%{searchQueryForWhereClause}%")
                    );
            }

            return await new LocalConveyanceExpenseList(_mapper).Create(collectionBeforePaging,
              expenseResource.Skip,
              expenseResource.PageSize);

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
