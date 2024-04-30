using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllEmpGradeQuery : IRequest<EmpGradeList>
    {
        public EmpGradeResource EmpGradeResource { get; set; }

    }
}
