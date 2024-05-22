using BTTEM.Data.Dto;
using BTTEM.MediatR.Dashboard.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var users = await _userRepository.FindAsync(request.UserId);
            dashboardStatics.UserName = string.Concat(users.FirstName, " ", users.LastName);
            dashboardStatics.TotalTrip = await _tripRepository.All.Where(x => x.CreatedBy == request.UserId).CountAsync();
            dashboardStatics.TotalAdvanceMoney = await _userRepository.All.Where(x => x.Id == request.UserId && x.IsPermanentAdvance == true).SumAsync(x => x.PermanentAdvance.Value);
            dashboardStatics.TotalExpenseAmount = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId).SumAsync(x => x.TotalAmount);
            dashboardStatics.TotalReimbursementAmount = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId).SumAsync(x => x.ReimbursementAmount);
            dashboardStatics.TotalApprovedExpense = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.ReimbursementStatus == "APPROVED").CountAsync();
            dashboardStatics.TotalPartialApprovedExpense = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.ReimbursementStatus == "PARTIAL").CountAsync();
            dashboardStatics.TotalPendingExpense = await _masterExpenseRepository.All.Where(x => x.CreatedBy == request.UserId && x.ReimbursementStatus == "PENDIND").CountAsync();
            return dashboardStatics;
        }
    }
}
