using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class MMTCity : BaseEntity
    {
        public Guid Id { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
    }
}
