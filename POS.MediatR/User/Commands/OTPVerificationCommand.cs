using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.User.Commands
{
    public class OTPVerificationCommand : IRequest<ServiceResponse<bool>>
    {
        public string UserName { get; set; }
        public string OTP { get; set; }
    }
}
