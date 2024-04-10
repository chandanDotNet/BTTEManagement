using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data
{
    public class PoliciesDetail : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GradeId { get; set; }
        public string Description { get; set; }
        public decimal DailyAllowance { get; set; }
        public string Document { get; set; }
        public Grade Grade { get; set; }
        public bool IsActive { get; set; }

    }
}
