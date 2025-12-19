using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class MMTCityResource : ResourceParameters
    {
        public MMTCityResource() : base("CityName")
        {

        }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
    }
}
