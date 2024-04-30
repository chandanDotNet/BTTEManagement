using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Handlers
{
    public class GetAllEmpGradeQueryHandler : IRequestHandler<GetAllEmpGradeQuery, EmpGradeList>
    {

        private readonly IEmpGradeRepository _empGradeRepository;
        public GetAllEmpGradeQueryHandler(IEmpGradeRepository empGradeRepository)
        {
            _empGradeRepository = empGradeRepository;
        }

        public async Task<EmpGradeList> Handle(GetAllEmpGradeQuery request, CancellationToken cancellationToken)
        {
            return await _empGradeRepository.GetEmpGrades(request.EmpGradeResource);
        }
    }
}
