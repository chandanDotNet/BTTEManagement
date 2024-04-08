using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Department.Commands
{
    public class GetAllDepartmentCommand : IRequest<List<DepartmentDto>>
    {
        public Guid? Id { get; set; }

    }
}
