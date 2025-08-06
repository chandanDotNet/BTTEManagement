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
#pragma warning disable CS0108 // 'TravelDocumentResource.SearchQuery' hides inherited member 'ResourceParameters.SearchQuery'. Use the new keyword if hiding was intended.
        public string SearchQuery { get; set; }
#pragma warning restore CS0108 // 'TravelDocumentResource.SearchQuery' hides inherited member 'ResourceParameters.SearchQuery'. Use the new keyword if hiding was intended.

        public Guid? UserId { get; set; }
    }
}