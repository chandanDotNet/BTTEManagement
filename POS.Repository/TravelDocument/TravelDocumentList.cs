using BTTEM.Data.Dto;
using BTTEM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class TravelDocumentList : List<TravelDocumentDto>
    {
        public TravelDocumentList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public TravelDocumentList(List<TravelDocumentDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<TravelDocumentList> Create(IQueryable<TravelDocument> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new TravelDocumentList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<TravelDocument> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<TravelDocumentDto>> GetDtos(IQueryable<TravelDocument> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new TravelDocumentDto
                {
                   Id = c.Id,
                   DocNumber= c.DocNumber,
                   DocType= c.DocType,
                   FileName= c.FileName,
                   IssuedOn = c.IssuedOn,
                   IsVerified= c.IsVerified,
                   ReceiptPath= c.ReceiptPath,
                   UserId= c.UserId,
                   ValidTill = c.ValidTill

                }).ToListAsync();
            return entities;
        }
    }
}