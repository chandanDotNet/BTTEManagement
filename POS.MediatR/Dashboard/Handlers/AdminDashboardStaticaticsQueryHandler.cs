using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Dashboard.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Handlers
{
    public class AdminDashboardStaticaticsQueryHandler : IRequestHandler<AdminDashboardStaticaticsQuery, AdminDashboardStatics>
    {

        private readonly ITripRepository _tripRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IUserRepository _userRepository;

        public AdminDashboardStaticaticsQueryHandler(
            IMasterExpenseRepository masterExpenseRepository,
            IUserRepository userRepository,
            ITripRepository tripRepository
           )
        {
            _masterExpenseRepository = masterExpenseRepository;
            _userRepository = userRepository;
            _tripRepository = tripRepository;
        }

        public async Task<AdminDashboardStatics> Handle(AdminDashboardStaticaticsQuery request, CancellationToken cancellationToken)
        {
            var dashboardStatics = new AdminDashboardStatics();
           // var users = await _userRepository.FindAsync(request.UserId);

            if(request.CompanyAccountId.HasValue)
            {
                dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();

                dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus != "PENDING").SumAsync(x => x.ReimbursementAmount);

                dashboardStatics.TotalExpensePending = await _masterExpenseRepository.All.Include(u=>u.CreatedByUser).Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                dashboardStatics.TotalExpenseApproved = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();

            }
            else
            {
                dashboardStatics.TotalTrip = await _tripRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year).CountAsync();
                dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").SumAsync(x => x.TotalAmount);
                dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedByUser.CompanyAccountId == request.CompanyAccountId && x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ReimbursementStatus == "APPROVED").SumAsync(x => x.ReimbursementAmount);

                dashboardStatics.TotalExpensePending = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "PENDING").CountAsync();
                dashboardStatics.TotalExpenseApproved = await _masterExpenseRepository.All.Include(u => u.CreatedByUser).Where(x => x.CreatedDate.Month == request.Month && x.CreatedDate.Year == request.Year && x.ApprovalStage == "APPROVED").CountAsync();

            }

          
            return dashboardStatics;
        }


    }
}
