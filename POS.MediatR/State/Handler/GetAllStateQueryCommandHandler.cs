using BTTEM.MediatR.State.Command;
using BTTEM.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.State.Handler
{
    public class GetAllStateQueryCommandHandler : IRequestHandler<GetAllStateQueryCommand, StateList>
    {
        private readonly IStateRepository _stateRepository;
        public GetAllStateQueryCommandHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<StateList> Handle(GetAllStateQueryCommand request, CancellationToken cancellationToken)
        {
            return await _stateRepository.GetStates(request.StateResource);
        }
    }
}
