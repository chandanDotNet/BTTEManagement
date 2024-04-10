using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Data.Dto
{
    public class DepartmentDto
    {
         public Guid Id { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public Guid DepartmentHeadId { get; set; }
        public bool? IsActive { get; set; }
       // public UserDto User { get; set; }
        public string? DepartmentHeadName { get; set; }
        public long? DepartmentHeadCount { get; set; }

    }
}
