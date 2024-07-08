using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class GradeResource : ResourceParameters
    {
        public GradeResource() : base("GradeName")
        {
        }
        public Guid? Id { get; set; }
        public string GradeName { get; set; }
        public string Description { get; set; }
    }
}
