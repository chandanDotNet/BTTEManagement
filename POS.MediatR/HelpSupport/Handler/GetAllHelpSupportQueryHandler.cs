using BTTEM.MediatR.HelpSupport.Command;
using BTTEM.Repository;
using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.HelpSupport.Handler
{
    public class GetAllHelpSupportQueryHandler : IRequestHandler<GetAllHelpSupportQuery, HelpSupportList>
    {
        private readonly IHelpSupportRepository _helpSupportRepository;
        public GetAllHelpSupportQueryHandler(IHelpSupportRepository helpSupportRepository)
        {
            _helpSupportRepository = helpSupportRepository;
        }
        public async Task<HelpSupportList> Handle(GetAllHelpSupportQuery request, CancellationToken cancellationToken)
        {
            return await _helpSupportRepository.GetAllHelpSupport(request.HelpSupportResource);
        }
    }
}
