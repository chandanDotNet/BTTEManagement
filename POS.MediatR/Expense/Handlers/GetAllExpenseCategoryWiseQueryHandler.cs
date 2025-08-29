using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
using BTTEM.MediatR.Expense.Commands;
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
    public class GetAllExpenseCategoryWiseQueryHandler : IRequestHandler<GetAllExpenseCategoryWiseQuery, AllExpenseData>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllExpenseCategoryWiseQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetAllExpenseCategoryWiseQueryHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetAllExpenseCategoryWiseQueryHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<AllExpenseData> Handle(GetAllExpenseCategoryWiseQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            AllExpenseData allExpenseData = new AllExpenseData();
            string connectionString = _pathHelper.connectionStrings.Trim();

            List<TourTravelExpenseList> expenseDataList = new List<TourTravelExpenseList>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@MasterExpenseId", request.Id);

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
                    expenseData.Remarks = rdr["Remarks"].ToString();
                    expenseData.ExpenseNo = rdr["ExpenseNo"].ToString();
                    expenseData.TripNo = rdr["TripNo"].ToString();
                    expenseDataList.Add(expenseData);
                }
                con.Close();
            }

            return allExpenseData;

        }

        }
}
