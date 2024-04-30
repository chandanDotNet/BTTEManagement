using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Resources
{
    public class EmpGradeResource : ResourceParameters
    {

        public EmpGradeResource() : base("GradeName")
        {
        }
        public Guid Id { get; set; }
        public string GradeName { get; set; }
        public string Description { get; set; }
    }
}
