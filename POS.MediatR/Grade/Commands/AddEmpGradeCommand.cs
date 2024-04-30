using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddEmpGradeCommand : IRequest<ServiceResponse<EmpGradeDto>>
    {
        public string GradeName { get; set; }
        public string Description { get; set; }

    }
}
