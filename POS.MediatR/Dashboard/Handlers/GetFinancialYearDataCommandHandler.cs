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
    public class GetFinancialYearDataCommandHandler : IRequestHandler<GetFinancialYearDataCommand, AllFinancialYearData>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFinancialYearDataCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetFinancialYearDataCommandHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetFinancialYearDataCommandHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<AllFinancialYearData> Handle(GetFinancialYearDataCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            AllDashboardData allDashboardData = new AllDashboardData();
            string connectionString = _pathHelper.connectionStrings.Trim();


            //========= My Dashboard Data Details
            AllFinancialYearData allFinancialYearData = new AllFinancialYearData();        


            //Team Recent Expenses

            List<FinancialYearData> FinancialYearDataList = new List<FinancialYearData>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 7);               

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    FinancialYearData FinancialYearData = new FinancialYearData();
                    FinancialYearData.Id = (int)rdr["Id"];
                    FinancialYearData.StartDate = rdr["StartDate"].ToString();
                    FinancialYearData.EndDate = rdr["EndDate"].ToString();
                    FinancialYearData.Name = rdr["Name"].ToString();

                    FinancialYearDataList.Add(FinancialYearData);
                }
                con.Close();
            }


            allFinancialYearData.FinancialYear = FinancialYearDataList;          


            return allFinancialYearData;
        }
    }
}
