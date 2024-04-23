using BTTEM.Data;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetTravelDocumentQuery : IRequest<List<TravelDocumentDto>> 
    {

        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
    }
}
