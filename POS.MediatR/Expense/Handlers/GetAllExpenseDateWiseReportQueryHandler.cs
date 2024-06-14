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
    public class GetAllExpenseDateWiseReportQueryHandler : IRequestHandler<GetAllExpenseDateWiseReportQuery, List<ExpenseDto>>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllExpenseDateWiseReportQueryHandler> _logger;
        public GetAllExpenseDateWiseReportQueryHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetAllExpenseDateWiseReportQueryHandler> logger)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ExpenseDto>> Handle(GetAllExpenseDateWiseReportQuery request, CancellationToken cancellationToken)
        {
            //var expense = await _expenseRepository.FindAsync(request.MasterExpenseId);
            //if (expense == null)
            //{
            //    _logger.LogError("Expense not found");
            //  //  return ServiceResponse<List<ExpenseDto>>.Return404();
            //}
            //var expenseDto = _mapper.Map<List<ExpenseDto>>(expense);
            //return ServiceResponse<List<ExpenseDto>>.ReturnResultWith200(expenseDto);

           // string connectionString = "Server=10.200.109.231,1433;Database=NonCSD_29_01_2024;user=sa; password=Shyam@2023;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=FALSE";
            string connectionString = "data source=10.200.109.231,1433;Initial Catalog=BTTEManagement;user id=sa;password=Shyam@2023; TrustServerCertificate=True";

            TourTravelExpenseList expenseData = new TourTravelExpenseList();
            List<TourTravelExpenseList> expenseDataList = new List<TourTravelExpenseList>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    expenseData.Date = rdr["Date"].ToString();
                    expenseData.Description = rdr["Description"].ToString();
                    expenseData.Fare = (decimal)rdr["Fare"];
                    expenseData.LocalConvey = (decimal)rdr["LocalConvey"];
                    expenseData.Lodging = (decimal)rdr["Lodging"];
                    expenseData.Fooding = (decimal)rdr["Fooding"];
                    expenseData.DA = (decimal)rdr["DA"];
                    expenseData.Others = (decimal)rdr["Others"];
                    expenseData.Total = (decimal)rdr["Total"];
                    expenseData.Remarks =rdr["Remarks"].ToString();
                    expenseDataList.Add(expenseData);
                }
                con.Close();
            }

            List<ExpenseDto> expenseDtos = new List<ExpenseDto>();
            return expenseDataList;
        }
    }
}
