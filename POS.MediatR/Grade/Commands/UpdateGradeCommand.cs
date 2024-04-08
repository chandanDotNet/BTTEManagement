using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Grade.Commands
{
    public class UpdateGradeCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string GradeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
