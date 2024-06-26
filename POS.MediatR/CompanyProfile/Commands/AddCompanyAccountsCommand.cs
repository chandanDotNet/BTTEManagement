using BTTEM.Data.Dto;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Commands
{
    public class AddCompanyAccountsCommand : IRequest<ServiceResponse<bool>>
    {
        public List<CompanyAccountDto> CompanyAccountsList { get; set; } = new List<CompanyAccountDto>();
    }
}
