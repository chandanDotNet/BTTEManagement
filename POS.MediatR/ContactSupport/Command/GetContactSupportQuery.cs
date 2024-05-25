using BTTEM.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetContactSupportQuery : IRequest<List<ContactSupportDto>>
    {
        public Guid? AssignedTo { get; set; }
    }
}

