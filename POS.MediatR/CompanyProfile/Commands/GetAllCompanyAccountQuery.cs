using BTTEM.Data.Resources;
using BTTEM.Repository;
using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CompanyProfile.Commands
{
    public class GetAllCompanyAccountQuery : IRequest<CompanyAccountList>
    {
        public CompanyAccountResource CompanyAccountResource { get; set; }
    }
}


