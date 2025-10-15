using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Commands
{
    public class UpdateTripCommand : IRequest<ServiceResponse<bool>>
    {

        public Guid Id { get; set; }
        public string TripNo { get; set; }
        public string TripType { get; set; }
        public string Name { get; set; }
        public DateTime? TripStarts { get; set; }
        public DateTime? TripEnds { get; set; }
        public Guid? PurposeId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Approval { get; set; }
        public Guid? SourceCityId { get; set; }
        public Guid? DestinationCityId { get; set; }
        public string MultiCity { get; set; }
        public string ModeOfTrip { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsRequestAdvanceMoney { get; set; }
        public decimal? AdvanceMoney { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? RequestAdvanceMoneyStatus { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? AdvanceMoneyRemarks { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public DateTime? RequestAdvanceMoneyDate { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? PurposeFor { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? SourceCityName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? DestinationCityName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? DepartmentName { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public Guid? CompanyAccountId { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? VendorCode { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool IsGroupTrip { get; set; } = false;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? NoOfPerson { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public bool Consent { get; set; } = false;
        public bool IsTripEndNotConfirmed { get; set; } = false;
        public DateTime? TripAppliedOn { get; set; }
        public List<GroupTripDto> GroupTrips { get; set; }
    }
}
