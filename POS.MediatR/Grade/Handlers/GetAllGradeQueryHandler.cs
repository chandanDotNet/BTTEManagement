using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Handlers
{
    public class GetAllGradeQueryHandler : IRequestHandler<GetAllGradeQuery, GradeList>
    {

        private readonly IGradeRepository _gradeRepository;
        public GetAllGradeQueryHandler(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }
        public async Task<GradeList> Handle(GetAllGradeQuery request, CancellationToken cancellationToken)
        {
            return await _gradeRepository.GetGrades(request.GradeResource);
        }
    }
}
