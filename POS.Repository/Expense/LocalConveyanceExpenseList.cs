using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository.Expense
{
    public class LocalConveyanceExpenseList : List<LocalConveyanceExpenseDto>
    {
        IMapper _mapper;
        public LocalConveyanceExpenseList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
      

        public LocalConveyanceExpenseList(List<LocalConveyanceExpenseDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);           
            AddRange(items);
        }

        public async Task<LocalConveyanceExpenseList> Create(IQueryable<LocalConveyanceExpense> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);           
            var dtoPageList = new LocalConveyanceExpenseList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<LocalConveyanceExpense> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LocalConveyanceExpenseDto>> GetDtos(IQueryable<LocalConveyanceExpense> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new LocalConveyanceExpenseDto
                    {
                       Id = cs.Id,
                       Amount = cs.Amount,
                       ApproxKM = cs.ApproxKM,
                       ExpenseDate = cs.ExpenseDate,
                       From = cs.From,
                       To = cs.To,                     
                       ModeOfTransport = cs.ModeOfTransport,
                       Particular = cs.Particular,
                       Place = cs.Place,
                       GrandTotal = cs.GrandTotal,
                       Documents = _mapper.Map<List<LocalConveyanceExpenseDocumentDto>>(cs.Documents).ToList(),

                    })//.OrderByDescending(x => x.CreatedDate) 
                    .ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
             .Skip(skip)
             .Take(pageSize)
             .AsNoTracking()
             .Select(cs => new LocalConveyanceExpenseDto
             {
                 Id = cs.Id,
                 Amount = cs.Amount,
                 ApproxKM = cs.ApproxKM,
                 ExpenseDate = cs.ExpenseDate,
                 From = cs.From,
                 To = cs.To,
                 ModeOfTransport = cs.ModeOfTransport,
                 Particular = cs.Particular,
                 Place = cs.Place,
                 GrandTotal = cs.GrandTotal,
                 Documents = _mapper.Map<List<LocalConveyanceExpenseDocumentDto>>(cs.Documents).ToList(),
             })//.OrderByDescending(x => x.CreatedDate)
             .ToListAsync();
                return entities;
            }
        }

    }
}
