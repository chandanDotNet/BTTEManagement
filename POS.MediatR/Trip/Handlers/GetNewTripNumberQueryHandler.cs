using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Handlers
{
    public class GetNewTripNumberQueryHandler : IRequestHandler<GetNewTripNumberCommand, string>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;
        public GetNewTripNumberQueryHandler(ITripRepository tripRepository, IMapper mapper)
        {
            _tripRepository = tripRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(GetNewTripNumberCommand request, CancellationToken cancellationToken)
        {
            var lastSalesOrder = await _tripRepository.All.Where(t=>t.IsDeleted==false).OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
            if (lastSalesOrder == null)
            {
                return "TR#00001";
            }

            var lastSONumber = lastSalesOrder.TripNo;
            var soId = Regex.Match(lastSONumber, @"\d+").Value;
            var isNumber = int.TryParse(soId, out int soNumber);
            if (isNumber)
            {
                var newSoId = lastSONumber.Replace(soNumber.ToString(), "");
                return $"{newSoId}{soNumber + 1}";
            }
            else
            {
                return $"{lastSONumber}#00001";
            }
        }

    }
}
