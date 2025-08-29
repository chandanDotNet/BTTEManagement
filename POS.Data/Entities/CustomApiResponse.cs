using BTTEM.Data.Dto;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Entities
{
    public class CustomApiResponse
    {

        public Guid Id { get; set; }

    }

    public class TripDetailsData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<TripDto> Data { get; set; }
    }
    public class ExpenseCategoryData
    {
        public Guid ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }
        public decimal AllowedAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal DeviationAmount { get; set; }
        public decimal FoodingAllowance { get; set; }
        public List<ExpenseDto> ExpenseDtos { get; set; } = new List<ExpenseDto>();
    }

    public class ExpenseResponseData
    {
        public MasterExpenseDto MaseterExpense { get; set; }
        public List<ExpenseCategoryData> ExpenseCategories { get; set; } = new List<ExpenseCategoryData>();
    }

    public class UserInfoData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public UserInfoDetails userInfoDetails { get; set; }        
    }

    public class UserInfoDetails
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public string ProfilePhoto { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfJoining { get; set; }
        public string EmployeeCode { get; set; }
        public string SapCode { get; set; }
        public string PanNo { get; set; }
        public string AadhaarNo { get; set; }
        public Guid Department { get; set; }
        public string DepartmentName { get; set; }
        public Guid GradeId { get; set; }
        public string GradeName { get; set; }
        public string Designation { get; set; }
        public Guid CompanyAccountId { get; set; }
        public string CompanyAccountName { get; set; }
        public Guid CompanyAccountBranchId { get; set; }
        public string BranchName { get; set; }
        public Guid ReportingTo { get; set; }
        public string ReportingToName { get; set; }
        public bool? IsPermanentAdvance { get; set; }
        public decimal? PermanentAdvance { get; set; }
        public decimal? DA { get; set; }
        public bool? IsMetroCities { get; set; }
        public decimal? MetroCitiesUptoAmount { get; set; }
        public bool? OtherCities { get; set; }
        public decimal? OtherCitiesUptoAmount { get; set; }
        public bool? IsFoodActuals { get; set; }
        public decimal? BudgetAmount { get; set; }
        public decimal? FoodAmountWithoutBill { get; set; }
        public decimal? CarDieselRate { get; set; }
        public decimal? CarPetrolRate { get; set; }
        public decimal? BikeRate { get; set; }
        public string FrequentFlyerNumber { get; set; }
        public string TravelClass { get; set; }
        public int ApprovalLevel { get; set; }
        public bool IsCompanyVehicleUser { get; set; }
        public string AlternateEmail { get; set; }
        public string AccountTeam { get; set; }
        public bool IsDirector { get; set; }
        public int CalenderDays { get; set; }
        public string DeviceKey { get; set; }
        public bool IsDeviceTypeAndroid { get; set; }
        //public List<PoliciesVehicleConveyance> PoliciesVehicleConveyance { get; set; }

    }
    public class TourTravelExpenseReport
    {
        public TourTravelExpenseDetails TourTravelExpenseDetails { get; set; }
        public IList<TourTravelExpenseList> TourTravelExpenseList { get; set; } = new List<TourTravelExpenseList>();
        public IList<TicketBookCancelExpenseList> TicketBookCancelExpenseList { get; set; } = new List<TicketBookCancelExpenseList>();
    }

    public class TourTravelExpenseDetails
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string Branch { get; set; }
        public string Dept { get; set; }
        public string EmployeeName { get; set; }
        public string JourneyNo { get; set; }
        public string EmployeeId { get; set; }
        public string PlaceOfTravel { get; set; }
        public string BookingDate { get; set; }
        public string BookedBy { get; set; }
        public string DuratioOfTravelFrom { get; set; }
        public string DuratioOfTravelTo { get; set; }
        public string ApprovedBy { get; set; }
        public string ExpenseNo { get; set; }
        public string TripNo { get; set; }


    }
    public class TourTravelExpenseList
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public decimal Fare { get; set; }
        public decimal LocalConvey { get; set; }
        public decimal Lodging { get; set; }
        public decimal Fooding { get; set; }
        public decimal DA { get; set; }
        public decimal Others { get; set; }
        public decimal Total { get; set; }
        public string Remarks { get; set; }    
        public string ExpenseNo { get; set; }    
        public string TripNo { get; set; }    


    }
    public class TicketBookCancelExpenseList
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public decimal Fare { get; set; }
        public string CancelationReason { get; set; }
        public decimal AgentCharge { get; set; }   


    }


    public class AllReportDetails
    {
        public UserExpenseReportDetails UserExpenseReportDetails { get; set; }
        public IList<UserAllTripReportDetails> UserAllTripReportDetailsList { get; set; } = new List<UserAllTripReportDetails>();
        public IList<UserAllExpenseReportDetails> UserAllExpenseReportDetailsList { get; set; } = new List<UserAllExpenseReportDetails>();
    }
    public class UserExpenseReportDetails
    {
        public int NoOfTrips { get; set; }
        public int NoOfExpenses { get; set; }
        public decimal ApplyAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal ReimbursementAmount { get; set; }
    }
    public class UserAllTripReportDetails
    {
        public Guid Id { get; set; }
        public string TripNo { get; set; }
        public string TripName { get; set; }
        public string TripStarts { get; set; }
        public string TripEnds { get; set; }
        
    }
    public class UserAllExpenseReportDetails
    {
        public Guid Id { get; set; }
        public string ExpenseNo { get; set; }
        public string ExpenseType { get; set; }
        public decimal ApplyAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal ReimbursementAmount { get; set; }
    }

    public class YearlyExpenseReportList
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public decimal Amount { get; set; }
    }

    public class DashboardReportData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public DashboardData Data { get; set; }
    }

    public class ExistingExpenseByTripData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public bool IsExpenseExist { get; set; }
        public MasterExpense Data { get; set; }
    }

    public class HelpSupportResponse
    {
        public string Description { get; set; }
        public List<HelpSupportQuery> helpSupportQueries { get; set; } = new List<HelpSupportQuery>();
    }
    public class HelpSupportQuery
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }


    public class AllUser
    {
        public int id { get; set; }
        public int? company { get; set; }
        public string branch_name { get; set; }
        public string sap_personnel_no { get; set; }
        public int? employee_grade { get; set; }
        public int? department { get; set; }
        public int? designation { get; set; }
        public int? reporting_head { get; set; }
        public string pan_no { get; set; }
        public string aadhar_no { get; set; }
        public string address { get; set; }
        public string bank_account { get; set; }
        public string ifsc_code { get; set; }
        public int? user { get; set; }
        public string employee_name { get; set; }
        public string employee_code { get; set; }
        public string company_name { get; set; }
        public string official_email_id { get; set; }
        public string email { get; set; }
        public string grade { get; set; }
        public string personal_contact_no { get; set; }
        public string official_contact_no { get; set; }
        public string dob { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public DateTime date_of_joining { get; set; }
        public string reporting_head_name { get; set; }
        public string bank_name { get; set; }
        public bool is_staff { get; set; }
        public bool is_active { get; set; }
    }

    public class ShyamSteel
    {
        public int request_status { get; set; }
        public List<AllUser> all_users { get; set; }
        public string msg { get; set; }
    }

    public class UserDeleteResponse
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
    }

    public class ResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }       
    }

    public class LoginResponse
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public UserAuthDto Data { get; set; }
    }
    public class AppVersionUpdateResponseData
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public IList<AppVersionUpdateDto> Data { get; set; }
    }

    public class OtpResponseData
    {
        public string otp { get; set; }
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
    }
    public class HRMSLoginResponse
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public string accesskey { get; set; }
    }

    public class AllDashboardData
    {       
        public MyDashboardData MyDashboardData { get; set; }
        public TeamDashboardData TeamDashboardData { get; set; }
        public IList<UserAllTripReportDetails> UserAllTripReportDetailsList { get; set; } = new List<UserAllTripReportDetails>();
        public IList<UserAllExpenseReportDetails> UserAllExpenseReportDetailsList { get; set; } = new List<UserAllExpenseReportDetails>();
    }

    public class MyDashboardData
    {
        public int TotalTripCount { get; set; } = 0;
        public int TotalTripPendingCount { get; set; } = 0;
        public int TotalTripApproveCount { get; set; } = 0;

        public int TotalExpenseCount { get; set; } = 0;
        public int TotalExpensePendingCount { get; set; } = 0;
        public int TotalExpenseApproveCount { get; set; } = 0;

        public decimal PermanentAdvance { get; set; }
        
    }
    public class TeamDashboardData
    {
        public int TotalTripCount { get; set; } = 0;
        public int TotalTripPendingCount { get; set; } = 0;
        public int TotalTripApproveCount { get; set; } = 0;

        public int TotalExpenseCount { get; set; } = 0;
        public int TotalExpensePendingCount { get; set; } = 0;
        public int TotalExpenseApproveCount { get; set; } = 0;       

    }
    public class AllDashboardDataResponse
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public AllDashboardData Data { get; set; }
    }

    public class OverallExpensesReportDataResponse
    {
        public bool status { get; set; }
        public int StatusCode { get; set; }
        public string message { get; set; }
        public AllTypeReports Data { get; set; }
    }

    public class AllTypeReports
    {       
        public List<OverallExpensesReportData> OverallExpensesReport { get; set; }
        public List<OverallTripReportData> OverallTripReport { get; set; }
    }

    public class OverallExpensesReportData
    {
        public string CompanyName { get; set; }
        public long NoOfExpenses { get; set; }
        public decimal ExpensesAmount { get; set; }
        public long ApproveByRM { get; set; }
        public decimal ApproveAmount { get; set; }
        public long TotalReimbursementCount { get; set; }
        public decimal ReimbursementAmount { get; set; }
       
    }

    public class OverallTripReportData
    {
        public string CompanyName { get; set; }
        public long NoOfTripApplied { get; set; }
        public long NoOfTripApproved { get; set; }
        public long NoOfTripCompleted { get; set; }        

    }

    public class MessageRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
        public bool DeviceType { get; set; }
        public string UserId { get; set; }
        public string CustomKey { get; set; }
        public string Id { get; set; }
    }

    public class AllExpenseData
    {
       
        public IList<AllExpenseDataList> AllExpenseDataList { get; set; } = new List<AllExpenseDataList>();
       
    }

    public class AllExpenseDataList
    {
        public Guid MasterExpenseId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }       
        public string ExpenseNo { get; set; }
        public Guid? TripId { get; set; }
        public string TripName { get; set; }
        public string ExpenseType { get; set; }
        public string Status { get; set; }
        public int NoOfBill { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeviationAmount { get; set; }
        public decimal? PayableAmount { get; set; }
        public decimal ReimbursementAmount { get; set; }       
        public string ApprovalStatus { get; set; }
        public Guid? ApprovalBy { get; set; }
        public string ApprovalByName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsGroupExpense { get; set; }
        public string NoOfPerson { get; set; }
        public string NoOfPersonName { get; set; }
        public List<AllExpenseCategoryWiseData> AllExpenseCategoryWise { get; set; } = new List<AllExpenseCategoryWiseData>();

    }

    public class AllExpenseCategoryWiseData
    {
        public Guid ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }
        public decimal AllowedAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal DeviationAmount { get; set; }       
        public List<ExpenseDto> ExpenseDtos { get; set; } = new List<ExpenseDto>();
    }
}
