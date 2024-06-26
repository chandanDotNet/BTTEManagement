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
    public class AddDepartmentListCommand : IRequest<ServiceResponse<bool>>
    {
        public List<DepartmentDto> DepartmentList { get; set; } = new List<DepartmentDto>();
    }
}