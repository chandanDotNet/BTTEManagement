using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
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
    public class GetAllExpenseDateWiseReportQueryHandler : IRequestHandler<GetAllExpenseDateWiseReportQuery, TourTravelExpenseReport>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllExpenseDateWiseReportQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetAllExpenseDateWiseReportQueryHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetAllExpenseDateWiseReportQueryHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<TourTravelExpenseReport> Handle(GetAllExpenseDateWiseReportQuery request, CancellationToken cancellationToken)
        {
            TourTravelExpenseReport tourTravelExpenseReport=new TourTravelExpenseReport();
            string connectionString = _pathHelper.connectionStrings.Trim();

            // string connectionString = "Server=10.200.109.231,1433;Database=NonCSD_29_01_2024;user=sa; password=Shyam@2023;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=FALSE";
           // string connectionString = "data source=10.200.109.231,1433;Initial Catalog=BTTEManagement;user id=sa;password=Shyam@2023; TrustServerCertificate=True";

           
            List<TourTravelExpenseList> expenseDataList = new List<TourTravelExpenseList>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@MasterExpenseId", request.MasterExpenseId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TourTravelExpenseList expenseData = new TourTravelExpenseList();
                    expenseData.Date = rdr["ExpenseDate"].ToString();
                    expenseData.Description = rdr["Description"].ToString();
                    expenseData.Fare = (decimal)rdr["Fare"];
                    expenseData.LocalConvey = (decimal)rdr["LocalConvey"];
                    expenseData.Lodging = (decimal)rdr["Lodging"];
                    expenseData.Fooding = (decimal)rdr["Fooding"];
                    expenseData.DA = (decimal)rdr["DA"];
                    expenseData.Others = (decimal)rdr["Others"];
                    expenseData.Total = (decimal)rdr["Total"];
                    expenseData.Remarks =rdr["Remarks"].ToString();
                    expenseData.ExpenseNo = rdr["ExpenseNo"].ToString();
                    expenseData.TripNo = rdr["TripNo"].ToString();
                    expenseDataList.Add(expenseData);
                }
                con.Close();
            }

            //========= User Details


            TourTravelExpenseDetails expenseUserData = new TourTravelExpenseDetails();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@MasterExpenseId", request.MasterExpenseId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    expenseUserData.CompanyId = rdr["CompanyId"].ToString();
                    expenseUserData.CompanyName = rdr["CompanyName"].ToString();
                    expenseUserData.CompanyLogo = rdr["CompanyLogo"].ToString();
                    expenseUserData.Branch = rdr["Branch"].ToString();
                    expenseUserData.Dept = rdr["Dept"].ToString();
                    expenseUserData.EmployeeName = rdr["EmployeeName"].ToString();
                    expenseUserData.JourneyNo = rdr["JourneyNo"].ToString();
                    expenseUserData.EmployeeId = rdr["EmployeeId"].ToString();
                    expenseUserData.PlaceOfTravel = rdr["PlaceOfTravel"].ToString();
                    expenseUserData.BookingDate = rdr["BookingDate"].ToString();
                    expenseUserData.BookedBy = rdr["BookedBy"].ToString();
                    expenseUserData.DuratioOfTravelFrom = rdr["DuratioOfTravelFrom"].ToString();
                    expenseUserData.DuratioOfTravelTo = rdr["DuratioOfTravelTo"].ToString();
                    expenseUserData.ApprovedBy = rdr["ApprovedBy"].ToString();
                    expenseUserData.ExpenseNo = rdr["ExpenseNo"].ToString();
                    expenseUserData.TripNo = rdr["TripNo"].ToString();
                   
                   // expenseDataList.Add(expenseData); 
                }
                con.Close();
            }

            List<TicketBookCancelExpenseList> ticketBookCancelExpenseList = new List<TicketBookCancelExpenseList>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 3);
                cmd.Parameters.AddWithValue("@MasterExpenseId", request.MasterExpenseId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TicketBookCancelExpenseList expenseData = new TicketBookCancelExpenseList();
                    expenseData.Date = rdr["Date"].ToString();
                    expenseData.Status = rdr["Status"].ToString();
                    expenseData.Description = rdr["Description"].ToString();
                    expenseData.Fare = (decimal)rdr["Fare"];
                    expenseData.CancelationReason = (string)rdr["CancelationReason"];
                    expenseData.AgentCharge = (decimal)rdr["AgentCharge"];

                    ticketBookCancelExpenseList.Add(expenseData);
                }
                con.Close();
            }

            tourTravelExpenseReport.TourTravelExpenseDetails = expenseUserData;
            tourTravelExpenseReport.TourTravelExpenseList = expenseDataList;
            tourTravelExpenseReport.TicketBookCancelExpenseList = ticketBookCancelExpenseList;

            //List<ExpenseDto> expenseDtos = new List<ExpenseDto>(); 
            return tourTravelExpenseReport;
        }
    }
}
