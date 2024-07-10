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
    public class CarBikeLogBookExpenseList : List<CarBikeLogBookExpenseDto>
    {

        IMapper _mapper;
        public CarBikeLogBookExpenseList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }


        public CarBikeLogBookExpenseList(List<CarBikeLogBookExpenseDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CarBikeLogBookExpenseList> Create(IQueryable<CarBikeLogBookExpense> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new CarBikeLogBookExpenseList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<CarBikeLogBookExpense> source)
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

        public async Task<List<CarBikeLogBookExpenseDto>> GetDtos(IQueryable<CarBikeLogBookExpense> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new CarBikeLogBookExpenseDto
                    {
                        Id = cs.Id,                        
                        ExpenseDate = cs.ExpenseDate,
                        From = cs.From,
                        To = cs.To,
                        ConsumptionKMS = cs.ConsumptionKMS,
                        EndingKMS = cs.EndingKMS,
                        FuelBillNo = cs.FuelBillNo,
                        PlaceOfVisitDepartment = cs.PlaceOfVisitDepartment,
                        RefillingAmount = cs.RefillingAmount,
                        RefillingLiters = cs.RefillingLiters,
                        StartingKMS=cs.StartingKMS,                       
                        CreatedDate = cs.CreatedDate,
                        MasterExpenseId = cs.MasterExpenseId,
                        Documents = _mapper.Map<List<CarBikeLogBookExpenseDocumentDto>>(cs.Documents).ToList(),
                        CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,

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
             .Select(cs => new CarBikeLogBookExpenseDto
             {
                 Id = cs.Id,
                 ExpenseDate = cs.ExpenseDate,
                 From = cs.From,
                 To = cs.To,
                 ConsumptionKMS = cs.ConsumptionKMS,
                 EndingKMS = cs.EndingKMS,
                 FuelBillNo = cs.FuelBillNo,
                 PlaceOfVisitDepartment = cs.PlaceOfVisitDepartment,
                 RefillingAmount = cs.RefillingAmount,
                 RefillingLiters = cs.RefillingLiters,
                 StartingKMS = cs.StartingKMS,
                 CreatedDate = cs.CreatedDate,
                 MasterExpenseId = cs.MasterExpenseId,
                 Documents = _mapper.Map<List<CarBikeLogBookExpenseDocumentDto>>(cs.Documents).ToList(),
                 CreatedByUser = cs.CreatedByUser != null ? _mapper.Map<UserDto>(cs.CreatedByUser) : null,
             })//.OrderByDescending(x => x.CreatedDate)
             .ToListAsync();
                return entities;
            }
        }
    }
}
