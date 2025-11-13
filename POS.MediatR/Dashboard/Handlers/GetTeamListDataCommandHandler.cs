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
    public class GetTeamListDataCommandHandler : IRequestHandler<GetTeamListDataCommand, AllTeamListData>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTeamListDataCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetTeamListDataCommandHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetTeamListDataCommandHandler> logger,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<AllTeamListData> Handle(GetTeamListDataCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
           
            string connectionString = _pathHelper.connectionStrings.Trim();

            //========= My Team List Data Details
            AllTeamListData allTeamListData = new AllTeamListData();


           
            List<TeamListData> TeamListDataList = new List<TeamListData>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_AllDashboardData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", 8);
                cmd.Parameters.AddWithValue("@UsersId", request.ID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TeamListData TeamListData = new TeamListData();
                    TeamListData.Id = (Guid)rdr["Id"];
                    TeamListData.Name = rdr["Name"].ToString();
                    TeamListData.EmailId = rdr["EmailId"].ToString();
                    TeamListData.CompanyName = rdr["CompanyName"].ToString();
                    TeamListData.Branch = rdr["Branch"].ToString();
                    TeamListData.Department = rdr["Department"].ToString();
                    TeamListData.Designation = rdr["Designation"].ToString();
                    TeamListData.SapCode = rdr["SapCode"].ToString();
                    TeamListData.EmpId = rdr["EmpId"].ToString();
                    TeamListData.MobileNo = rdr["MobileNo"].ToString();
                    TeamListData.Grade = rdr["Grade"].ToString();

                    TeamListDataList.Add(TeamListData);
                }
                con.Close();
            }


            allTeamListData.TeamListData = TeamListDataList;


            return allTeamListData;
        }

    }
}
