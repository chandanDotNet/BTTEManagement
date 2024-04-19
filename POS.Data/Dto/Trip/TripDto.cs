using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TripDto
    {

        public Guid Id { get; set; }
       // public string TripNumber { get; set; }
        public string TripNo { get; set; }
        public string TripType { get; set; }
        public string Name { get; set; }
        public DateTime TripStarts { get; set; }
        public DateTime TripEnds { get; set; }
        public Guid PurposeId { get; set; }
        public string Description { get; set; }
        public PurposeDto Purpose { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
