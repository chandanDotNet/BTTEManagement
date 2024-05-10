using POS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class TripTracking : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid ActionBy { get; set; }
        [ForeignKey("ActionBy")]
        public User ActionByUser { get; set; }
        public DateTime ActionDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
    }
}
