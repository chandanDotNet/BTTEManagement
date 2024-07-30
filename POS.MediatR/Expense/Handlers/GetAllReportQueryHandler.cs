using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
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

namespace BTTEM.MediatR.Expense.Handlers
{
    public class GetAllReportQueryHandler : IRequestHandler<GetAllReportQuery, AllReportDetails>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllReportQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetAllReportQueryHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetAllReportQueryHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<AllReportDetails> Handle(GetAllReportQuery request, CancellationToken cancellationToken)
        {
            AllReportDetails allReportDetails = new AllReportDetails();
            string connectionString = _pathHelper.connectionStrings.Trim();



            //========= User All Trip Report Details
            List<UserAllTripReportDetails> userAllTripReportDetailsList = new List<UserAllTripReportDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllReports", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@FromDate", request.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", request.ToDate);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UserAllTripReportDetails userAllTripReportDetails = new UserAllTripReportDetails();
                    userAllTripReportDetails.Id = (Guid)rdr["Id"];
                    userAllTripReportDetails.TripNo = rdr["TripNo"].ToString();
                    userAllTripReportDetails.TripName = rdr["TripName"].ToString();
                    userAllTripReportDetails.TripStarts = rdr["TripStarts"].ToString();
                    userAllTripReportDetails.TripEnds = rdr["TripEnds"].ToString();                   
                    userAllTripReportDetailsList.Add(userAllTripReportDetails);
                }
                con.Close();
            }

            //========= User All Expense Report Details
            List<UserAllExpenseReportDetails> userAllExpenseReportDetailsList = new List<UserAllExpenseReportDetails>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllReports", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 3);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@FromDate", request.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", request.ToDate);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();  

                while (rdr.Read())
                {
                    UserAllExpenseReportDetails userAllExpenseReportDetails = new UserAllExpenseReportDetails();
                    userAllExpenseReportDetails.Id = (Guid)rdr["Id"];
                    userAllExpenseReportDetails.ExpenseNo = rdr["ExpenseNo"].ToString();
                    userAllExpenseReportDetails.ExpenseType = rdr["ExpenseType"].ToString();
                    userAllExpenseReportDetails.ApplyAmount = (decimal)rdr["ApplyAmount"];
                    userAllExpenseReportDetails.PayableAmount = (decimal)rdr["PayableAmount"];
                    userAllExpenseReportDetails.ReimbursementAmount = (decimal)rdr["ReimbursementAmount"];
                    userAllExpenseReportDetailsList.Add(userAllExpenseReportDetails);
                }
                con.Close(); 
            }

            //========= User Expense Report Details
            UserExpenseReportDetails userExpenseReportDetails = new UserExpenseReportDetails();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllReports", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);
                cmd.Parameters.AddWithValue("@FromDate", request.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", request.ToDate);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    userExpenseReportDetails.NoOfTrips =(int) rdr["NoOfTrips"];
                    userExpenseReportDetails.NoOfExpenses = (int) rdr["NoOfExpenses"];
                    userExpenseReportDetails.ApplyAmount = (decimal) rdr["ApplyAmount"];
                    userExpenseReportDetails.PayableAmount = (decimal) rdr["PayableAmount"];
                    userExpenseReportDetails.ReimbursementAmount = (decimal) rdr["ReimbursementAmount"];                   

                }
                con.Close();
            }


            allReportDetails.UserExpenseReportDetails = userExpenseReportDetails;
            allReportDetails.UserAllTripReportDetailsList = userAllTripReportDetailsList;
            allReportDetails.UserAllExpenseReportDetailsList = userAllExpenseReportDetailsList;

            //List<ExpenseDto> expenseDtos = new List<ExpenseDto>(); 
            return allReportDetails;
        }

    }
}
