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


        public async Task<AllTypeReports> Handle(GetOverallExpensesReportDataCommand request, CancellationToken cancellationToken)
        {
            AllTypeReports allTypeReports = new AllTypeReports();
            List <OverallExpensesReportData> overallExpensesReportDataList = new List<OverallExpensesReportData>();
            string connectionString = _pathHelper.connectionStrings.Trim();


            //========= My Dashboard Data Details
           
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

            allTypeReports.OverallExpensesReport= overallExpensesReportDataList;

            return allTypeReports;
        }
    }
}
