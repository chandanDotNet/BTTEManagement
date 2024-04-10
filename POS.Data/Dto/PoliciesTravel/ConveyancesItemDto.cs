using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ConveyancesItemDto
    {

        public Guid Id { get; set; }
        public Guid ConveyanceId { get; set; }
        public string ConveyanceItemName { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsCheck { get; set; }
    }
}
