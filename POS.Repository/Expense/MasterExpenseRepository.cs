using AutoMapper;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Data;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTTEM.Repository.Expense;
using BTTEM.Data.Resources;

namespace BTTEM.Repository
{
    public class MasterExpenseRepository : GenericRepository<MasterExpense, POSDbContext>,
            IMasterExpenseRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        public MasterExpenseRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
             UserInfoToken userInfoToken,
              IUserRoleRepository userRoleRepository,
              IUserRepository userRepository
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        public async Task<MasterExpenseList> GetAllExpenses(ExpenseResource expenseResource)
        {

            Guid LoginUserId = Guid.Parse(_userInfoToken.Id);
            var Role = GetUserRole(LoginUserId).Result.FirstOrDefault();

            if (!expenseResource.MasterExpenseId.HasValue)
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


            //var collectionBeforePaging = AllIncluding(c => c.CreatedByUser).ApplySort(expenseResource.OrderBy,
            //    _propertyMappingService.GetPropertyMapping<MasterExpenseDto, MasterExpense>());

            var collectionBeforePaging = All.Include(t => t.Trip).ThenInclude(g => g.GroupTrips)
                .Include(g => g.GroupExpenses).ThenInclude(u => u.User).Include(c => c.CreatedByUser).
                ThenInclude(e => e.Grades).Include(a => a.Expenses).ThenInclude(e => e.ExpenseDocument).
                Include(a => a.Expenses).ThenInclude(c => c.ExpenseCategory).ApplySort(expenseResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<MasterExpenseDto, MasterExpense>());

            //var collectionBeforePaging = AllIncluding(c => c.CreatedByUser, a => a.Expenses).ApplySort(expenseResource.OrderBy,
            //   _propertyMappingService.GetPropertyMapping<MasterExpenseDto, MasterExpense>());

            //.ProjectTo<TripDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (expenseResource.ReportingHeadId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedByUser.ReportingTo == expenseResource.ReportingHeadId);
            }
            if (expenseResource.CreatedBy.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedBy == expenseResource.CreatedBy);
            }

            if (expenseResource.TripId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.TripId == expenseResource.TripId);
            }

            if (expenseResource.MasterExpenseId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Id == expenseResource.MasterExpenseId);
            }

            if (expenseResource.ExpenseById.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedBy == expenseResource.ExpenseById);
            }

            if (expenseResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c => c.ExpenseDate >= new DateTime(expenseResource.FromDate.Value.Year, expenseResource.FromDate.Value.Month, expenseResource.FromDate.Value.Day, 0, 0, 1)));
            }
            if (expenseResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c => c.ExpenseDate <= new DateTime(expenseResource.ToDate.Value.Year, expenseResource.ToDate.Value.Month, expenseResource.ToDate.Value.Day, 23, 59, 59)));
            }

            if (expenseResource.ExpenseCategoryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c => c.ExpenseCategoryId == expenseResource.ExpenseCategoryId));
            }
            if (!string.IsNullOrEmpty(expenseResource.Status))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == expenseResource.Status);
            }
            if (!string.IsNullOrEmpty(expenseResource.ApprovalStage))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ApprovalStage == expenseResource.ApprovalStage);
            }

            if (!string.IsNullOrEmpty(expenseResource.ExpenseType))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseType == expenseResource.ExpenseType);
            }

            if (!string.IsNullOrEmpty(expenseResource.ExpenseByUser))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseByUser == expenseResource.ExpenseByUser);
            }
            if (!string.IsNullOrEmpty(expenseResource.ExpenseStatus))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c => c.Status == expenseResource.ExpenseStatus));
            }

            if (!string.IsNullOrEmpty(expenseResource.BranchName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedByUser.BranchName == expenseResource.BranchName);
            }

            if (expenseResource.BranchId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CreatedByUser.CompanyAccountBranchId == expenseResource.BranchId);
            }
            if (!string.IsNullOrEmpty(expenseResource.JourneyNumber))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.JourneyNumber == expenseResource.JourneyNumber);
            }

            collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Expenses.Any(c => c.Amount > 0));

            if (!string.IsNullOrEmpty(expenseResource.SearchQuery))
            {
                var searchQueryForWhereClause = expenseResource.SearchQuery
              .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                    EF.Functions.Like(a.ExpenseNo, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Name, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.ExpenseType, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.ApprovalStage, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Status, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.Trip.Name, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.TotalAmount.ToString(), $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.PayableAmount.ToString(), $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.JourneyNumber.ToString(), $"%{searchQueryForWhereClause}%")

                    );
            }


            return await new MasterExpenseList(_mapper, _userRepository).Create(collectionBeforePaging,
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
