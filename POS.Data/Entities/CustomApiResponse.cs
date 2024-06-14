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


    }
    public class TourTravelExpenseList
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public string Fare { get; set; }
        public string LocalConvey { get; set; }
        public string Lodging { get; set; }
        public string Fooding { get; set; }
        public string DA { get; set; }
        public string Others { get; set; }
        public string Total { get; set; }
        public string Remarks { get; set; }    


    }



}
