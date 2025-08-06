using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using BTTEM.MediatR.Dashboard.Commands;
using BTTEM.MediatR.Expense.Handlers;
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
    public class GetAllDashboardDataQueryHandler : IRequestHandler<GetAllDashboardDataQueryCommand, AllDashboardData>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllDashboardDataQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetAllDashboardDataQueryHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetAllDashboardDataQueryHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<AllDashboardData> Handle(GetAllDashboardDataQueryCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            AllDashboardData allDashboardData = new AllDashboardData();
            string connectionString = _pathHelper.connectionStrings.Trim();
                      

            //========= My Dashboard Data Details
            MyDashboardData myDashboardData = new MyDashboardData();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    myDashboardData.TotalTripCount = (int)rdr["TotalTripCount"];
                    myDashboardData.TotalTripPendingCount = (int)rdr["TotalTripPendingCount"];
                    myDashboardData.TotalTripApproveCount = (int)rdr["TotalTripApproveCount"];
                    myDashboardData.TotalExpenseCount = (int)rdr["TotalExpenseCount"];
                    myDashboardData.TotalExpensePendingCount = (int)rdr["TotalExpensePendingCount"];
                    myDashboardData.TotalExpenseApproveCount = (int)rdr["TotalExpenseApproveCount"];
                    myDashboardData.PermanentAdvance = (decimal)rdr["PermanentAdvance"];                  

                }
                con.Close();
            }

            //========= Team Dashboard Data Details
            TeamDashboardData teamDashboardData = new TeamDashboardData();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    teamDashboardData.TotalTripCount = (int)rdr["TotalTripCount"];
                    teamDashboardData.TotalTripPendingCount = (int)rdr["TotalTripPendingCount"];
                    teamDashboardData.TotalTripApproveCount = (int)rdr["TotalTripApproveCount"];
                    teamDashboardData.TotalExpenseCount = (int)rdr["TotalExpenseCount"];
                    teamDashboardData.TotalExpensePendingCount = (int)rdr["TotalExpensePendingCount"];
                    teamDashboardData.TotalExpenseApproveCount = (int)rdr["TotalExpenseApproveCount"];
                   

                }
                con.Close();
            }


            allDashboardData.MyDashboardData = myDashboardData;
            allDashboardData.TeamDashboardData = teamDashboardData;       

            
            return allDashboardData;
        }

    }
}
