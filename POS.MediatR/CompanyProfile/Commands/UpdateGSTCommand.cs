using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Commands
{
    public class UpdateGSTCommand : IRequest<ServiceResponse<CompanyProfileDto>>
    {
        public string GST { get; set; }
        public bool Registration { get; set; }
    }
}
