using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Commands
{
    public class AddGradeListCommand : IRequest<ServiceResponse<bool>>
    {
        public List<GradeDto> GradeList { get; set; } = new List<GradeDto>();
    }
}
