using AutoMapper;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;
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

namespace BTTEM.MediatR.User.Handlers
{
    public class GetUserInfoDetailsQueryHandler : IRequestHandler<GetUserInfoDetailsQuery, UserInfoData>
    {

        private readonly PathHelper _pathHelper;
        public GetUserInfoDetailsQueryHandler(
            PathHelper pathHelper)
        {

            _pathHelper = pathHelper;
        }

        public async Task<UserInfoData> Handle(GetUserInfoDetailsQuery request, CancellationToken cancellationToken)
        {
            UserInfoData userInfoData = new UserInfoData();
            string connectionString = _pathHelper.connectionStrings.Trim();

            UserInfoDetails userInfoDetails = new UserInfoDetails();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 4);
                cmd.Parameters.AddWithValue("@UsersId", request.UserId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                 
                while (rdr.Read())
                {

                    userInfoDetails.UserId = (Guid)rdr["UserId"];
                    userInfoDetails.UserFullName = rdr["UserFullName"].ToString();
                    userInfoDetails.ProfilePhoto = rdr["ProfilePhoto"].ToString();
                    userInfoDetails.Address = rdr["Address"].ToString();
                    userInfoDetails.UserName = rdr["UserName"].ToString();
                    userInfoDetails.Email = rdr["Email"].ToString();
                    userInfoDetails.PhoneNumber = rdr["PhoneNumber"].ToString();
                    userInfoDetails.DateOfJoining = rdr["DateOfJoining"].ToString();
                    userInfoDetails.EmployeeCode = rdr["EmployeeCode"].ToString();
                    userInfoDetails.SapCode = rdr["SapCode"].ToString();
                    userInfoDetails.PanNo = rdr["PanNo"].ToString();
                    userInfoDetails.AadhaarNo = rdr["AadhaarNo"].ToString();
                    userInfoDetails.Department =(Guid) rdr["Department"];
                    userInfoDetails.DepartmentName = rdr["DepartmentName"].ToString();
                    userInfoDetails.GradeId =(Guid) rdr["GradeId"];
                    userInfoDetails.GradeName = (string) rdr["GradeName"];
                    userInfoDetails.Designation = rdr["Designation"].ToString();
                    userInfoDetails.CompanyAccountId = (Guid)rdr["CompanyAccountId"];
                    userInfoDetails.CompanyAccountName = rdr["CompanyAccountName"].ToString();
                    userInfoDetails.CompanyAccountBranchId = (Guid)rdr["CompanyAccountBranchId"];
                    userInfoDetails.BranchName = rdr["BranchName"].ToString();
                    userInfoDetails.ReportingTo =(Guid) rdr["ReportingTo"];
                    userInfoDetails.ReportingToName = rdr["ReportingToName"].ToString();
                    userInfoDetails.IsPermanentAdvance = (bool)rdr["IsPermanentAdvance"];
                    userInfoDetails.PermanentAdvance = (decimal)rdr["PermanentAdvance"];                  
                    userInfoDetails.DA = (decimal)rdr["DA"];
                    userInfoDetails.IsMetroCities = (bool)rdr["IsMetroCities"];
                    userInfoDetails.MetroCitiesUptoAmount = (decimal)rdr["MetroCitiesUptoAmount"];
                    userInfoDetails.OtherCities = (bool)rdr["OtherCities"];
                    userInfoDetails.OtherCitiesUptoAmount = (decimal)rdr["OtherCitiesUptoAmount"];
                    userInfoDetails.IsFoodActuals = (bool)rdr["IsFoodActuals"];
                    userInfoDetails.BudgetAmount = (decimal)rdr["BudgetAmount"];
                    userInfoDetails.FoodAmountWithoutBill = (decimal)rdr["FoodAmountWithoutBill"];
                    userInfoDetails.FrequentFlyerNumber = Convert.ToString(rdr["FrequentFlyerNumber"]);
                    userInfoDetails.TravelClass = Convert.ToString(rdr["TravelClass"]);
                    // expenseDataList.Add(expenseData);
                }
                con.Close();
            }

            userInfoData.userInfoDetails = userInfoDetails;
            userInfoData.status = true;
            userInfoData.StatusCode = 200;

            return userInfoData;
        }

    }
}
