using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class TripTrackingResource : ResourceParameters
    {
        public TripTrackingResource() : base("CreatedDate")
        {
        }
        public Guid? TripId { get; set; }
        public Guid? TripItineraryId { get; set; }
        public string ActionType { get; set; }
    }
}
