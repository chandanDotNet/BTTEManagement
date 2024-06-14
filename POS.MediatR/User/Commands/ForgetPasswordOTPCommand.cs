using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.User.Commands
{
    public class ForgetPasswordOTPCommand : IRequest<ServiceResponse<string>>
    {
        public string UserName { get; set; }
    }
}
