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
using System.IO;
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
                    myDashboardData.TotalTripRejectCount = (int)rdr["TotalTripRejectCount"];
                    myDashboardData.TotalTripCanceledCount = (int)rdr["TotalTripCanceledCount"];
                    myDashboardData.TotalExpenseCount = (int)rdr["TotalExpenseCount"];
                    myDashboardData.TotalExpensePendingCount = (int)rdr["TotalExpensePendingCount"];
                    myDashboardData.TotalExpenseApproveCount = (int)rdr["TotalExpenseApproveCount"];
                    myDashboardData.TotalExpenseRejectCount = (int)rdr["TotalExpenseRejectCount"];
                    myDashboardData.TotalExpenseRembCount = (int)rdr["TotalExpenseRembCount"];
                    myDashboardData.PermanentAdvance = (decimal)rdr["PermanentAdvance"];  
                    
                    myDashboardData.Name = rdr["Name"].ToString();                  
                    myDashboardData.EmpCode = rdr["EmpCode"].ToString();                  
                    myDashboardData.Grade = rdr["Grade"].ToString();                  
                    myDashboardData.ProfilePhoto = rdr["ProfilePhoto"].ToString();                  
                    //myDashboardData.ProfilePhoto = Path.Combine(_pathHelper.UserProfilePath, rdr["ProfilePhoto"].ToString());                  

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
                    teamDashboardData.TotalTripRejectCount = (int)rdr["TotalTripRejectCount"];
                    teamDashboardData.TotalTripCanceledCount = (int)rdr["TotalTripCanceledCount"];
                    teamDashboardData.TotalExpenseCount = (int)rdr["TotalExpenseCount"];
                    teamDashboardData.TotalExpensePendingCount = (int)rdr["TotalExpensePendingCount"];
                    teamDashboardData.TotalExpenseApproveCount = (int)rdr["TotalExpenseApproveCount"];
                    teamDashboardData.TotalExpenseRejectCount = (int)rdr["TotalExpenseRejectCount"];
                    teamDashboardData.TotalExpenseRembCount = (int)rdr["TotalExpenseRembCount"];
                   

                }
                con.Close();
            }

            //My Upcoming Trip

            List<MyUpcomingTripDetails> MyUpcomingTripDetailsList = new List<MyUpcomingTripDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 3);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    MyUpcomingTripDetails MyUpcomingTripDetails = new MyUpcomingTripDetails();
                    MyUpcomingTripDetails.Id = (Guid)rdr["Id"];
                    MyUpcomingTripDetails.TripNo = rdr["TripNo"].ToString();
                    MyUpcomingTripDetails.TripName = rdr["TripName"].ToString();
                    MyUpcomingTripDetails.TripStarts = rdr["TripStarts"].ToString();
                    MyUpcomingTripDetails.TripEnds = rdr["TripEnds"].ToString();
                    MyUpcomingTripDetails.SourceCityName = rdr["SourceCityName"].ToString();
                    MyUpcomingTripDetails.DestinationCityName = rdr["DestinationCityName"].ToString();
                    MyUpcomingTripDetails.AppliedOn = rdr["AppliedOn"].ToString();
                    MyUpcomingTripDetails.AppliedBy = rdr["AppliedBy"].ToString();
                    MyUpcomingTripDetails.TripBy = rdr["TripBy"].ToString();

                    MyUpcomingTripDetailsList.Add(MyUpcomingTripDetails);
                }
                con.Close();
            }

            //Team Upcoming Trip

            List<TeamUpcomingTripDetails> TeamUpcomingTripDetailsList = new List<TeamUpcomingTripDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 4);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TeamUpcomingTripDetails TeamUpcomingTripDetails = new TeamUpcomingTripDetails();
                    TeamUpcomingTripDetails.Id = (Guid)rdr["Id"];
                    TeamUpcomingTripDetails.TripNo = rdr["TripNo"].ToString();
                    TeamUpcomingTripDetails.TripName = rdr["TripName"].ToString();
                    TeamUpcomingTripDetails.TripStarts = rdr["TripStarts"].ToString();
                    TeamUpcomingTripDetails.TripEnds = rdr["TripEnds"].ToString();
                    TeamUpcomingTripDetails.SourceCityName = rdr["SourceCityName"].ToString();
                    TeamUpcomingTripDetails.DestinationCityName = rdr["DestinationCityName"].ToString();
                    TeamUpcomingTripDetails.AppliedOn = rdr["AppliedOn"].ToString();
                    TeamUpcomingTripDetails.AppliedBy = rdr["AppliedBy"].ToString();
                    TeamUpcomingTripDetails.TripBy = rdr["TripBy"].ToString();

                    TeamUpcomingTripDetailsList.Add(TeamUpcomingTripDetails);
                }
                con.Close();
            }


            //My Recent Expenses

            List<MyRecentExpensesDetails> MyRecentExpensesDetailsList = new List<MyRecentExpensesDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 5);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    MyRecentExpensesDetails MyRecentExpensesDetails = new MyRecentExpensesDetails();
                    MyRecentExpensesDetails.Id = (Guid)rdr["Id"];
                    MyRecentExpensesDetails.ExpenseNo = rdr["ExpenseNo"].ToString();
                    MyRecentExpensesDetails.Status = rdr["Status"].ToString();
                    MyRecentExpensesDetails.Approval = rdr["Approval"].ToString();
                    MyRecentExpensesDetails.TotalAmount = (decimal)rdr["TotalAmount"];
                    MyRecentExpensesDetails.ApprovedAmount = (decimal)rdr["ApprovedAmount"];
                    MyRecentExpensesDetails.ReimbursementAmount = (decimal)rdr["ReimbursementAmount"];
                    MyRecentExpensesDetails.AppliedOn = rdr["AppliedOn"].ToString();
                    MyRecentExpensesDetails.AppliedBy = rdr["AppliedBy"].ToString();
                  

                    MyRecentExpensesDetailsList.Add(MyRecentExpensesDetails);
                }
                con.Close();
            }


            //Team Recent Expenses

            List<TeamRecentExpensesDetails> TeamRecentExpensesDetailsList = new List<TeamRecentExpensesDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 6);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@Month", request.Month);
                cmd.Parameters.AddWithValue("@Year", request.Year);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TeamRecentExpensesDetails TeamRecentExpensesDetails = new TeamRecentExpensesDetails();
                    TeamRecentExpensesDetails.Id = (Guid)rdr["Id"];
                    TeamRecentExpensesDetails.ExpenseNo = rdr["ExpenseNo"].ToString();
                    TeamRecentExpensesDetails.Status = rdr["Status"].ToString();
                    TeamRecentExpensesDetails.Approval = rdr["Approval"].ToString();
                    TeamRecentExpensesDetails.TotalAmount = (decimal)rdr["TotalAmount"];
                    TeamRecentExpensesDetails.ApprovedAmount = (decimal)rdr["ApprovedAmount"];
                    TeamRecentExpensesDetails.ReimbursementAmount = (decimal)rdr["ReimbursementAmount"];
                    TeamRecentExpensesDetails.AppliedOn = rdr["AppliedOn"].ToString();
                    TeamRecentExpensesDetails.AppliedBy = rdr["AppliedBy"].ToString();

                    TeamRecentExpensesDetailsList.Add(TeamRecentExpensesDetails);
                }
                con.Close();
            }


            allDashboardData.MyDashboardData = myDashboardData;
            allDashboardData.TeamDashboardData = teamDashboardData;       
            allDashboardData.MyUpcomingTripDetailsList = MyUpcomingTripDetailsList;       
            allDashboardData.TeamUpcomingTripDetailsList = TeamUpcomingTripDetailsList;       
            allDashboardData.MyRecentExpensesDetailsList = MyRecentExpensesDetailsList;       
            allDashboardData.TeamRecentExpensesDetailsList = TeamRecentExpensesDetailsList;       

            
            return allDashboardData;
        }

    }
}
