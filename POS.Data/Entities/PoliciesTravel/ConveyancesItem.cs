using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ConveyancesItem
    {
        public Guid Id { get; set; }
        public Guid ConveyanceId { get; set; }
        [ForeignKey("ConveyanceId")]
        public string ConveyanceItemName { get; set; }        
        public bool? IsCheck { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
