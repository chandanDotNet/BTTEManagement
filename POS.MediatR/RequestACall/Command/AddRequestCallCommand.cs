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
    public class AddRequestCallCommand : IRequest<ServiceResponse<RequestCallDto>>
    {
        public string RequestText { get; set; }
        public string ContactNo { get; set; }
        public string DocumentName { get; set; }
        public string DocumentData { get; set; }
        public DateTime RequestCallDate { get; set; }
        public string RequestCallTime { get; set; }
    }
}
