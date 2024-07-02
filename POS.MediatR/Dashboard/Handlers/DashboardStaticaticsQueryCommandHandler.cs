using BTTEM.Data.Dto;
using BTTEM.MediatR.Dashboard.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Handlers
{
    public class DashboardStaticaticsQueryCommandHandler : IRequestHandler<DashboardStaticaticsQueryCommand, DashboardData>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUserRepository _userRepository;

        public DashboardStaticaticsQueryCommandHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUserRepository userRepository,
            ITripRepository tripRepository
           )
        {
            _masterExpenseRepository = masterExpenseRepository;
            _userRepository = userRepository;
            _tripRepository = tripRepository;
        }

        public async Task<DashboardData> Handle(DashboardStaticaticsQueryCommand request, CancellationToken cancellationToken)
        {
            var dashboardStatics = new DashboardData();
            var users =  _userRepository.All.Include(x => x.UserRoles).Where(y => y.Id == request.UserId).FirstOrDefault();
            //if(users == null)
            //{
            var RoleId = users.UserRoles.FirstOrDefault().RoleId;
            
            if(RoleId== new Guid("F8B6ACE9-A625-4397-BDF8-F34060DBD8E4")) //Super Admin
            {
                if (request.CompanyAccountId.HasValue)
                {
                    dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                    dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                    dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus != "PENDING").SumAsync(x => x.ReimbursementAmount);
                    dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                    dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();

                }
                else
                {
                    dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                    dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                    dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus == "APPROVED").SumAsync(x => x.ReimbursementAmount);
                    dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                    dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();

                }
            }
            else if (RoleId == new Guid("1683EE71-D0B0-443C-AF0E-3AFFA589D0FA")) //Admin
            {
                if (request.CompanyAccountId.HasValue)
                {
                    dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                    dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                    dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus != "PENDING").SumAsync(x => x.ReimbursementAmount);
                    dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                    dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();

                }
                else
                {
                    dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                    dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                    dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus == "APPROVED").SumAsync(x => x.ReimbursementAmount);
                    dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                    dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();

                }
            }
            else if(RoleId== new Guid("E1BD3DCE-EECF-468D-B930-1875BD59D1F4")) //Employee
            {
                dashboardStatics.UserName = string.Concat(users.FirstName, " ", users.LastName);
                dashboardStatics.TotalTrip = await _tripRepository.All.Where(x => x.CreatedBy == request.UserId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                dashboardStatics.TotalAdvanceMoney = await _userRepository.All.Where(x => x.Id == request.UserId && x.IsPermanentAdvance == true).SumAsync(x => x.PermanentAdvance.Value);
                dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus != "PENDING").SumAsync(x => x.ReimbursementAmount);
                dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();
                dashboardStatics.TotalPartialApprovedExpense = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus == "PARTIAL").CountAsync();
                dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
            }
            else if (RoleId == new Guid("F9B4CCD2-6E06-443C-B964-23BF935F859E")) //Reporting Manager
            {
                dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.CreatedByUser.ReportingTo == request.UserId).CountAsync();
                dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED" && x.CreatedByUser.ReportingTo == request.UserId).SumAsync(x => x.TotalAmount);
                dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus == "APPROVED" && x.CreatedByUser.ReportingTo == request.UserId).SumAsync(x => x.ReimbursementAmount);
                dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING" && x.CreatedByUser.ReportingTo == request.UserId).CountAsync();
                dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED" && x.CreatedByUser.ReportingTo == request.UserId).CountAsync();
                dashboardStatics.TotalAdvanceMoney = await _userRepository.All.Where(x => x.Id == request.UserId && x.IsPermanentAdvance == true).SumAsync(x => x.PermanentAdvance.Value);

            }
            else if (RoleId == new Guid("F72616BE-260B-41BB-A4EE-89146622179A")) //Travel Desk
            {
                //
            }
            else if (RoleId == new Guid("241772CB-C907-4961-88CB-A0BF8004BBB2")) //Accounts
            {
                request.CompanyAccountId = users.CompanyAccountId;
                dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                        .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus != "PENDING").SumAsync(x => x.ReimbursementAmount);
                dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();
            }
            else
            {
                dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser)
                       .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus == "APPROVED").SumAsync(x => x.ReimbursementAmount);
                dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Include(u => u.CreatedByUser)
                    .Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();
            }

            
            return dashboardStatics;
        }
    }
}
