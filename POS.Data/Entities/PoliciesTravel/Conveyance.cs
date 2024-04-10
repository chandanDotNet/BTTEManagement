using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class Conveyance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }        
        public bool? IsMaster { get; set; }
        public Guid? PoliciesDetailId { get; set; }
        public bool? IsDeleted { get; set; }
        public List<ConveyancesItem> conveyancesItem { get; set; }
    }
}
