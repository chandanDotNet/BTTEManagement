using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Dashboard.Commands;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Dashboard.Handlers
{
    public class GetOverallExpensesReportDataHandler : IRequestHandler<GetOverallExpensesReportDataCommand, AllTypeReports>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOverallExpensesReportDataHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetOverallExpensesReportDataHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetOverallExpensesReportDataHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }


#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<AllTypeReports> Handle(GetOverallExpensesReportDataCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            AllTypeReports allTypeReports = new AllTypeReports();
            List <OverallExpensesReportData> overallExpensesReportDataList = new List<OverallExpensesReportData>();
            List <OverallTripReportData> overallTripReportDataList = new List<OverallTripReportData>();
            string connectionString = _pathHelper.connectionStrings.Trim();


            //========= Overall Expenses Report Data

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_CompanyExpensesReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@CompanyId", request.CompanyId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    OverallExpensesReportData overallExpensesReportData = new OverallExpensesReportData();
                    overallExpensesReportData.CompanyName = (string)rdr["CompanyName"];
                    overallExpensesReportData.NoOfExpenses = (long)rdr["NoOfExpenses"];
                    overallExpensesReportData.ExpensesAmount = (decimal)rdr["ExpensesAmount"];
                    overallExpensesReportData.ApproveByRM = (long)rdr["ApproveByRM"];
                    overallExpensesReportData.ApproveAmount = (decimal)rdr["ApproveAmount"];
                    overallExpensesReportData.TotalReimbursementCount = (long)rdr["TotalReimbursementCount"];
                    overallExpensesReportData.ReimbursementAmount = (decimal)rdr["ReimbursementAmount"];
                    overallExpensesReportDataList.Add(overallExpensesReportData);
                }
                con.Close();
            }

            //========= Overall Trip Report Data

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_CompanyExpensesReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@CompanyId", request.CompanyId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    OverallTripReportData overallTripReportData = new OverallTripReportData();
                    overallTripReportData.CompanyName = (string)rdr["CompanyName"];
                    overallTripReportData.NoOfTripApplied = (long)rdr["NoOfTripApplied"];
                    overallTripReportData.NoOfTripApproved = (long)rdr["NoOfTripApproved"];
                    overallTripReportData.NoOfTripCompleted = (long)rdr["NoOfTripCompleted"];
                    overallTripReportDataList.Add(overallTripReportData);
                }
                con.Close();
            }

            allTypeReports.OverallExpensesReport= overallExpensesReportDataList;
            allTypeReports.OverallTripReport= overallTripReportDataList;

            return allTypeReports;
        }
    }
}
