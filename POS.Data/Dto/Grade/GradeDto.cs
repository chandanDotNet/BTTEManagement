using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class GradeDto
    {

        public Guid Id { get; set; }
        public string GradeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long? NoOfUsers { get; set; }
        public string GradeCode { get; set; }
    }
}
