
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class ConveyanceDto
    {

        public Guid Id { get; set; }       
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public List<ConveyancesItemDto> conveyancesItem { get; set; }
    }
}
