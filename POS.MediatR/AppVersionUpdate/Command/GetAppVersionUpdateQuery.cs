using BTTEM.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.MediatR.AppVersionUpdate.Command
{
    public class GetAppVersionUpdateQuery : IRequest<List<AppVersionUpdateDto>>
    {

    }
}
