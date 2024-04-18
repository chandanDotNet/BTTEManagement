using BTTEM.Data;
using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.CommandAndQuery
{
    public class GetAllPurposeQuery : IRequest<List<PurposeDto>>
    {


    }
}
