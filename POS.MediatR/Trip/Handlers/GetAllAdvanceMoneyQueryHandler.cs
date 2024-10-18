using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Trip.Handlers
{
    public class GetAllAdvanceMoneyQueryHandler : IRequestHandler<GetAllAdvanceMoneyQuery, AdvanceMoneyList>
    {
        private readonly IAdvanceMoneyRepository _advanceMoneyRepository;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        public GetAllAdvanceMoneyQueryHandler(IAdvanceMoneyRepository advanceMoneyRepository, IMapper mapper, UserInfoToken userInfoToken)
        {
            _advanceMoneyRepository = advanceMoneyRepository;
            _mapper = mapper;
            _userInfoToken = userInfoToken;
        }
        public async Task<AdvanceMoneyList> Handle(GetAllAdvanceMoneyQuery request, CancellationToken cancellationToken)
        {
            return await _advanceMoneyRepository.GetAllAdvanceMoney(request.AdvanceMoneyResource);            
        }
    }
}