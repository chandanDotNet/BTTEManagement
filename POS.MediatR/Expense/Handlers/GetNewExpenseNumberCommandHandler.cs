using AutoMapper;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BTTEM.MediatR.Expense.Handlers
{
    public class GetNewExpenseNumberCommandHandler : IRequestHandler<GetNewExpenseNumberCommand, string>
    {

        private readonly IMasterExpenseRepository _masterExpenseRepository;
        private readonly IMapper _mapper;
        public GetNewExpenseNumberCommandHandler(IMasterExpenseRepository masterExpenseRepository, IMapper mapper)
        {
            _masterExpenseRepository = masterExpenseRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(GetNewExpenseNumberCommand request, CancellationToken cancellationToken)
        {
            var lastExpOrder = await _masterExpenseRepository.All.Where(t => t.IsDeleted == false).OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
            if (lastExpOrder == null)
            {
                return "EX#00001";
            }

            var lastSONumber = lastExpOrder.ExpenseNo;
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
