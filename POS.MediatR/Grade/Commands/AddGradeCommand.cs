using BTTEM.Data;
using BTTEM.Data.Dto;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddGradeCommand : IRequest<ServiceResponse<GradeDto>>
    {
       
        public string GradeName { get; set; }
        public string Description { get; set; }

    }
}
