using BTTEM.Data.Dto;
using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class Trip : BaseEntity
    {

        public Guid Id { get; set; }
        //public string TripNumber { get; set; }
        public string TripNo { get; set; }    
        public string TripType { get; set; } 
        public string Name { get; set; }
        public DateTime TripStarts { get; set; }
        public DateTime TripEnds { get; set; }
        public Guid PurposeId { get; set; }
        public string Description { get; set; }
        public Purpose Purpose { get; set; }
        // public User User { get; set; }
        public string Status { get; set; }
        public string Approval { get; set; }
        public Guid SourceCityId { get; set; }
        public City SourceCity { get; set; }
        public Guid DestinationCityId { get; set; }
        public City DestinationCity { get; set; }
        public string MultiCity { get; set; }
        public string ModeOfTrip { get; set; }
        public bool IsRequestAdvanceMoney { get; set; }
        public decimal? AdvanceMoney { get; set; }
        public string? RequestAdvanceMoneyStatus { get; set; }
        public DateTime? RequestAdvanceMoneyDate { get; set; }
        public Guid? RequestAdvanceMoneyStatusBy { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<TripItinerary>TripItinerarys { get; set; }
        public bool IsTripCompleted { get; set; }
        public string? PurposeFor { get; set; }
        public string? SourceCityName { get; set; }
        public string? DestinationCityName { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? CompanyAccountId { get; set; }
    }
}
