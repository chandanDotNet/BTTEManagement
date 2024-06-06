using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class TripTrackingDto
    {
        public Guid Id { get; set; }
        public Guid? TripId { get; set; }
        public Guid? TripItineraryId { get; set; }
        public Guid ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionByName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string ActionType { get; set; }
        public string TripTypeName { get; set; }
        public string ImageUrl { get; set; }
    }
}
