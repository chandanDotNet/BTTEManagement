using BTTEM.Data;
using BTTEM.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetRequestCallQuery : IRequest<List<RequestCallDto>>
    {
        public Guid? AssignedTo { get; set; }
    }
}

