using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ClassOfTravel
    {

        public Guid Id { get; set; }
        public Guid TravelModeId { get; set; }
        [ForeignKey("TravelModeId")]
        //public TravelMode TravelMode { get; set; }
        public string ClassName { get; set; }
        public bool IsCheck { get; set; }
        public bool IsDeleted { get; set; }
    }
}
