using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TravelMode
    {
        public Guid Id { get; set; }
        public string TravelsModesName { get; set; }
        public string TravelsModesImage { get; set; }
        public Guid? PoliciesDetailId { get; set; }
        public bool IsMaster { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCheck { get; set; }
        public List<ClassOfTravel> classOfTravels { get; set; }

    }
}
