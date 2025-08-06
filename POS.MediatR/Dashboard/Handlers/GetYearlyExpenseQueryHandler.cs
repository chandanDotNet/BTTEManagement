using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Expense.Handlers;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.MediatR.Dashboard.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Handlers
{
    public class GetYearlyExpenseQueryHandler : IRequestHandler<GetYearlyExpenseQuery, List<YearlyExpenseReportList>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMasterExpenseRepository _masterExpenseRepository;

        public GetYearlyExpenseQueryHandler(IExpenseRepository expenseRepository, IMasterExpenseRepository masterExpenseRepository)
        {
            _expenseRepository = expenseRepository;
            _masterExpenseRepository = masterExpenseRepository;

        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<List<YearlyExpenseReportList>> Handle(GetYearlyExpenseQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            List<YearlyExpenseReportList> yearlyExpenseReportList = new List<YearlyExpenseReportList>();
            

            var monthlyData = _masterExpenseRepository.All.Include(a=>a.CreatedByUser).Where(a => a.CreatedDate.Year == request.Year && a.Status == "REIMBURSED" && a.CreatedByUser.CompanyAccountId== request.CompanyAccountId)
                .GroupBy(a => a.CreatedDate.Month)
                .Select(cs => new YearlyExpenseReportList
                {
                    Month = cs.FirstOrDefault().CreatedDate.Month,
                    MonthName = cs.FirstOrDefault().CreatedDate.ToString("MMMM"),
                    Amount = cs.Sum(item => item.PayableAmount).Value
                })
                .OrderBy(a => a.Month)
                .ToList();

            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                         .TakeWhile(m => m != String.Empty)
                         .Select((m, i) => new
                         {
                             Month = i + 1,
                             MonthName = m
                         })
                         .ToList();

            foreach (var month in months)
            {
                YearlyExpenseReportList yearlyExpenseReport = new YearlyExpenseReportList();
                yearlyExpenseReport.Month = month.Month;
                yearlyExpenseReport.MonthName = month.MonthName;
                yearlyExpenseReport.Amount = monthlyData.Where(b => b.Month == month.Month).Select(a => a.Amount).FirstOrDefault();
                yearlyExpenseReportList.Add(yearlyExpenseReport);
            }
           
            return yearlyExpenseReportList;
        }
    }
}
