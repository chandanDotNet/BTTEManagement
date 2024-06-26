﻿using BTTEM.Data.Dto;
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
        public List<ExpenseDto> ExpenseDtos { get; set; } = new List<ExpenseDto>();
    }

    public class ExpenseResponseData
    {
        public MasterExpenseDto MaseterExpense { get; set; }
        public IList<ExpenseCategoryData> ExpenseCategories { get; set; } = new List<ExpenseCategoryData>();
    }
    public class TourTravelExpenseReport
    {
        public TourTravelExpenseDetails TourTravelExpenseDetails { get; set; }
        public IList<TourTravelExpenseList> TourTravelExpenseList { get; set; } = new List<TourTravelExpenseList>();
    }

    public class TourTravelExpenseDetails
    {
        public string CompanyName { get; set; }
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

}
