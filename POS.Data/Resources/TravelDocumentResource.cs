using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class TravelDocumentResource : ResourceParameters
    {
        public TravelDocumentResource() : base("CreatedDate")
        {
        }       
        public string SearchQuery { get; set; }

        public Guid? UserId { get; set; }
    }
}