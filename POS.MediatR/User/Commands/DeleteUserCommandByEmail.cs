using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Commands
{
    public class DeleteUserCommandByEmail : IRequest<ServiceResponse<UserDto>>
    {
        public string Email { get; set; }
    }
}
