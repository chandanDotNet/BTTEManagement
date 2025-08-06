using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using POS.MediatR.Tax.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Tax.Commands;
using POS.Repository;

namespace BTTEM.MediatR.Tax.Handlers
{
    public class GetAllTaxCodeCommandHandler : IRequestHandler<GetAllTaxCodeCommand, List<TaxCodeDto>>
    {
        private readonly ITaxCodeRepository _taxCodeRepository;
        private readonly IMapper _mapper;
        public GetAllTaxCodeCommandHandler(
           ITaxCodeRepository taxCodeRepository,
            IMapper mapper)
        {
            _taxCodeRepository = taxCodeRepository;
            _mapper = mapper;
        }

        public async Task<List<TaxCodeDto>> Handle(GetAllTaxCodeCommand request, CancellationToken cancellationToken)
        {
            var entities = await _taxCodeRepository.All.ProjectTo<TaxCodeDto>(_mapper.ConfigurationProvider).ToListAsync();
            return entities;
        }
    }
}
