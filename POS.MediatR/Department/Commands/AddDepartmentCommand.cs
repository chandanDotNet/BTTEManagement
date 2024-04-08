using BTTEM.Data;
using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Department.Commands
{
    public class AddDepartmentCommand : IRequest<ServiceResponse<DepartmentDto>>
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public Guid DepartmentHeadId { get; set; }
        public bool IsActive { get; set; }
        
    }
}
