using AutoMapper;
using AutoMapper.QueryableExtensions;
using BTTEM.Data;
using BTTEM.Data.Dto.PoliciesTravel;
using BTTEM.MediatR.PoliciesTravel.Commands;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.PoliciesTravel.Handlers
{
    public class GetUserGradeAndAccountCommandHandler : IRequestHandler<GetUserGradeAndAccountCommand, UserGradeAndAccountDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;

        public GetUserGradeAndAccountCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken

            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;

        }

        public async Task<UserGradeAndAccountDto> Handle(GetUserGradeAndAccountCommand request, CancellationToken cancellationToken)
        {
            var userGradeAndAccountDto = new UserGradeAndAccountDto();
            var result = await _userRepository.All.Where(c => c.Id == request.UserId).FirstOrDefaultAsync();
            userGradeAndAccountDto.GradeId = result.GradeId;
            userGradeAndAccountDto.CompanyAccountId = result.CompanyAccountId;
            return userGradeAndAccountDto;
        }

    }
}
