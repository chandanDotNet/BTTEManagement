using BTTEM.Data;
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
    public class AddContactSupportCommand : IRequest<ServiceResponse<ContactSupportDto>>
    {
        public string QueryText { get; set; }
        public int ContactNo { get; set; }
        public string DocumentName { get; set; }
        public string DocumentData { get; set; }
        public int IssueNo { get; set; }
        public string IssueText { get; set; }
        public string CustomText { get; set; }
    }
}
