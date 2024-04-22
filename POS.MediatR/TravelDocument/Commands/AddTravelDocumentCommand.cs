using BTTEM.Data;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class AddTravelDocumentCommand : IRequest<ServiceResponse<TravelDocumentDto>>
    {

        public string FileName { get; set; }
        public string DocumentData { get; set; }
        public string DocType { get; set; }
        public string DocNumber { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ValidTill { get; set; }
        public bool IsVerified { get; set; }
    }
}
