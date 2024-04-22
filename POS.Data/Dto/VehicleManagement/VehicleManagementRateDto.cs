using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class VehicleManagementRateDto
    {
        public Guid Id { get; set; }
        public Guid VehicleManagementId { get; set; }
        public Guid GradeId { get; set; }
        public string GradeName { get; set; }
        public string Rates { get; set; }
        public bool SameForAll { get; set; }
        public bool GradeSpecific { get; set; }
        public bool IsActive { get; set; }
        public int Serial { get; set; }
        public Grade Grade { get; set; }
    }
}
