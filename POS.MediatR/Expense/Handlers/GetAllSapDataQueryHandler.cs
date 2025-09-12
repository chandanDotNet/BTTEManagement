using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Expense.Commands;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
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
    public class GetAllSapDataQueryHandler : IRequestHandler<GetAllSapDataQuery, Record>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllSapDataQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetAllSapDataQueryHandler(
            IMapper mapper,
            ILogger<GetAllSapDataQueryHandler> logger,
            PathHelper pathHelper)
        {
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }
        public async Task<Record> Handle(GetAllSapDataQuery request, CancellationToken cancellationToken)
        {
            Record record = new Record();
            WithTax withTax = new WithTax();
            string connectionString = _pathHelper.connectionStrings.Trim();            

            //Action - 1
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_SAPDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 1);
                cmd.Parameters.AddWithValue("@MasterExpenseId", request.MasterExpenseId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    withTax.Item.Add(new Item()
                    {
                        Supplier = Convert.ToString(rdr["Supplier"]),
                        Amount = Convert.ToString(rdr["Amount"]),
                        BaseAmount = Convert.ToString(rdr["BaseAmount"]),
                        BusinessArea = Convert.ToString(rdr["BusinessArea"]),
                        BusinessPlace = Convert.ToString(rdr["BusinessPlace"]),
                        CostCenter = Convert.ToString(rdr["CostCenter"]),
                        DocPostingDate = Convert.ToString(rdr["DocPostingDate"]),
                        ExpenseType = Convert.ToString(rdr["ExpenseType"]),
                        MainVendorIRNApplicable = Convert.ToString(rdr["MainVendorIRNApplicable"]),
                        RefDocNo = Convert.ToString(rdr["RefDocNo"]),
                        SACCode1 = Convert.ToString(rdr["SACCode1"]),
                        SACCode2 = Convert.ToString(rdr["SACCode2"]),
                        SubVendor = Convert.ToString(rdr["SubVendor"]),
                        SubVendorIRNApplicable = Convert.ToString(rdr["SubVendorIRNApplicable"]),
                        TaxCode = Convert.ToString(rdr["TaxCode"]),
                        TaxGSTSubVendor = Convert.ToString(rdr["TaxGSTSubVendor"]),

                    });
                }
                con.Close();
            }

            //Action - 2
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_SAPDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 2);
                cmd.Parameters.AddWithValue("@MasterExpenseId", request.MasterExpenseId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    record.AuthorizationKey = Convert.ToString(rdr["AuthorizationKey"]);
                    record.CompanyCode = Convert.ToString(rdr["CompanyCode"]);
                    record.UserName = Convert.ToString(rdr["UserName"]);
                    withTax.JourneyNo = Convert.ToString(rdr["JourneyNo"]);
                    withTax.StartDate = Convert.ToString(rdr["StartDate"]);
                    withTax.EndDate = Convert.ToString(rdr["EndDate"]);
                    withTax.DocumentDate = Convert.ToString(rdr["DocumentDate"]);
                    withTax.PostingDate = Convert.ToString(rdr["PostingDate"]);
                    withTax.HODate = Convert.ToString(rdr["HODate"]);
                    withTax.Persons = Convert.ToString(rdr["Persons"]);
                    withTax.TravelType = Convert.ToString(rdr["TravelType"]);
                    withTax.Reference = Convert.ToString(rdr["Reference"]);
                    withTax.TradingPartner = Convert.ToString(rdr["TradingPartner"]);
                    withTax.Assignment = Convert.ToString(rdr["Assignment"]);
                    withTax.Remarks = Convert.ToString(rdr["Remarks"]);
                    withTax.ActivityNo = Convert.ToString(rdr["ActivityNo"]);
                    withTax.BikeMaintenance = Convert.ToString(rdr["BikeMaintenance"]);
                    withTax.CreditNote = Convert.ToString(rdr["CreditNote"]);

                }
                con.Close();
            }
            record.WithTax = withTax;
            return record;
        }
    }
}
