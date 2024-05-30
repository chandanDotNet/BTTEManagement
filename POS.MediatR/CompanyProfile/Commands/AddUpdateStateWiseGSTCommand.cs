using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddUpdateStateWiseGSTCommand : IRequest<ServiceResponse<List<CompanyGSTDto>>>
    {
        public List<CompanyGSTDto> StateWiseGSTList { get; set; }
    }
}
