using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Dashboard.Commands;
using POS.API.Helpers;
using BTTEM.MediatR.Dashboard.Commands;
using System;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Entities;
using POS.Repository;

namespace POS.API.Controllers.Dashboard
{
    /// <summary>
    /// DashboardController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        public IMediator _mediator { get; set; }

        private readonly IUserRepository _userRepository;
        /// <summary>
        /// DashboardController
        /// </summary>
        /// <param name="mediator"></param>
        public DashboardController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets the dashboard statistics.
        /// </summary>
        /// <returns></returns>
        [HttpGet("statistics")]
        //[ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            var dashboardStaticaticsQuery = new DashboardStaticaticsQuery { };
            var result = await _mediator.Send(dashboardStaticaticsQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the daily reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("dailyreminder/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetDailyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetDailyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the weekly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("weeklyreminder/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetWeeklyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetWeeklyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the monthly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("monthlyreminder/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetMonthlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetMonthlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the quarterly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("quarterlyreminder/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetQuarterlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetQuarterlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the half yearly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("halfyearlyreminder/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetHalfYearlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetHalfYearlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the yearly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("yearlyreminder/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetYearlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetYearlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Custom Reminders.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("onetime/{month}/{year}")]
        [ClaimCheck("DB_STATISTICS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetOneTimeReminders(int month, int year)
        {
            var monthlyEventQuery = new GetOneTimeReminerQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Best Selling Products
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("bestsellingproduct/{month}/{year}")]
        [ClaimCheck("DB_BEST_SELLING_PROS")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> BestSellingProduct(int month, int year)
        {
            var monthlyEventQuery = new GetBestSellingProductCommand { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Sells vs Purchase Report.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("salesvspurchase/{month}/{year}")]
        [ClaimCheck("REP_SALES_VS_PURCHASE_REP")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> SalesVsPurchase(int month, int year)
        {
            var monthlyEventQuery = new GetSalesVsPurchaseReportCommand { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }


        /// <summary>
        /// Gets the dashboard statistics report.
        /// </summary>
        /// <returns></returns>
        [HttpPost("StatisticsReport")]
       // [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetDashboardStatisticsReport(DashboardStaticaticsQueryCommand  dashboardStaticaticsQueryCommand)
        {
            //var dashboardStaticaticsQueryCommand = new DashboardStaticaticsQueryCommand
            //{
            //    UserId = UserId,
            //    CompanyAccountId= CompanyAccountId,
            //    Month = month,DashboardData
            //    Year = year
            //};
            var result = await _mediator.Send(dashboardStaticaticsQueryCommand);
            return Ok(result);
        }

        /// <summary>
        /// Gets the dashboard statistics report for App.
        /// </summary>
        /// <returns></returns>
        [HttpPost("StatisticsReportForApp")]
        // [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetDashboardStatisticsReportForApp(DashboardStaticaticsQueryCommand dashboardStaticaticsQueryCommand)
        {
            DashboardReportData dashboardReportData=new DashboardReportData();
            dashboardStaticaticsQueryCommand.IsMy = true;
             var result = await _mediator.Send(dashboardStaticaticsQueryCommand);
            if(result != null)
            {
                dashboardReportData.status = true;
                dashboardReportData.StatusCode = 200;
                dashboardReportData.Data = result;
            }
            else
            {
                dashboardReportData.status = false;
                dashboardReportData.StatusCode = 500;
                dashboardReportData.Data = result;
            }
            return Ok(dashboardReportData);
        }

        /// <summary>
        /// Gets the Admin Dashboard Statistics.
        /// </summary>
        /// <returns></returns>
        [HttpPost("AdminDashboardStatistics")]
        //[ClaimCheck("DB_STATISTICS")]
        //[Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetAdminDashboardStatistics(AdminDashboardStaticaticsQuery adminDashboardStaticaticsQuery)
        {            
            var result = await _mediator.Send(adminDashboardStaticaticsQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the yearly reminders.
        /// </summary>        
        /// <param name="year">The year.</param>
        /// <param name="CompanyAccountId">Company Account.</param>
        /// <returns></returns>
        [HttpGet("GetYearlyExpenseReport/{year}/{CompanyAccountId}")]       
        //[Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetYearlyExpenseReport(int year,Guid CompanyAccountId)
        {
            var monthlyEventQuery = new GetYearlyExpenseQuery { Year = year, CompanyAccountId= CompanyAccountId };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }



        /// <summary>
        /// Gets all dashboard data for App and web.
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllDashboardData")]
        // [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetAllDashboardData(GetAllDashboardDataQueryCommand getAllDashboardDataQueryCommand)
        {
            AllDashboardDataResponse allDashboardDataResponse = new AllDashboardDataResponse();           
            var result = await _mediator.Send(getAllDashboardDataQueryCommand);
            if (result != null)
            {
                var userInfo = await _userRepository.FindAsync(getAllDashboardDataQueryCommand.UserId);
                var updateUserProfileCommand = new UpdateUserProfileCommand()
                {
                    Address = userInfo.Address,
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    PhoneNumber = userInfo.PhoneNumber,
                    UserName = userInfo.UserName,
                    DeviceKey= getAllDashboardDataQueryCommand.DeviceKey,   
                    IsDeviceTypeAndroid = getAllDashboardDataQueryCommand.IsDeviceTypeAndroid
                };
                var userResult = await _mediator.Send(updateUserProfileCommand);

                allDashboardDataResponse.status = true;
                allDashboardDataResponse.StatusCode = 200;
                allDashboardDataResponse.Data = result;
            }
            else
            {
                allDashboardDataResponse.status = false;
                allDashboardDataResponse.StatusCode = 500;
                allDashboardDataResponse.Data = result;
            }
            return Ok(allDashboardDataResponse);
        }


        /// <summary>
        /// Gets all Overall Expenses Report Data
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOverallExpensesReportData")]
        // [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetOverallExpensesReportData(GetOverallExpensesReportDataCommand getOverallExpensesReportDataCommand)
        {
            OverallExpensesReportDataResponse overallExpensesReportDataResponse = new OverallExpensesReportDataResponse();
            var result = await _mediator.Send(getOverallExpensesReportDataCommand);
            if (result != null)
            {
                overallExpensesReportDataResponse.status = true;
                overallExpensesReportDataResponse.StatusCode = 200;
                overallExpensesReportDataResponse.Data = result;
            }
            else
            {
                overallExpensesReportDataResponse.status = false;
                overallExpensesReportDataResponse.StatusCode = 500;
                overallExpensesReportDataResponse.Data = result;
            }
            return Ok(overallExpensesReportDataResponse);
        }


        /// <summary>
        /// Get Financial Year Data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFinancialYearData")]
        // [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetFinancialYearData()
        {

            GetFinancialYearDataCommand getFinancialYearDataCommand=new GetFinancialYearDataCommand();
            AllFinancialYearDataResponse allFinancialYearDataResponse = new AllFinancialYearDataResponse();           

            //GetFinancialYearDataCommandHandler
            var result = await _mediator.Send(getFinancialYearDataCommand);
            if (result != null)
            {
                allFinancialYearDataResponse.status = true;
                allFinancialYearDataResponse.StatusCode = 200;
                allFinancialYearDataResponse.Data = result;
            }
            else
            {
                allFinancialYearDataResponse.status = false;
                allFinancialYearDataResponse.StatusCode = 500;
                allFinancialYearDataResponse.Data = result;
            }
            return Ok(allFinancialYearDataResponse);
        }


        /// <summary>
        /// Get Team List Data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTeamListData")]
        // [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetTeamListData(Guid Id)
        {

            GetTeamListDataCommand getTeamListDataCommand = new GetTeamListDataCommand();
            getTeamListDataCommand.ID = Id;
            TeamListDataResponse teamListDataResponse = new TeamListDataResponse();

            //GetFinancialYearDataCommandHandler
            var result = await _mediator.Send(getTeamListDataCommand);
            if (result != null)
            {
                teamListDataResponse.status = true;
                teamListDataResponse.StatusCode = 200;
                teamListDataResponse.Data = result;
            }
            else
            {
                teamListDataResponse.status = false;
                teamListDataResponse.StatusCode = 500;
                teamListDataResponse.Data = result;
            }
            return Ok(teamListDataResponse);
        }

    }
}
