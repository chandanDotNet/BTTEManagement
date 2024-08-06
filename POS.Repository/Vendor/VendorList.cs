using BTTEM.Data;
using BTTEM.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTTEM.Repository
{
    public class VendorList : List<VendorDto>
    {
        public VendorList()
        {

        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public VendorList(List<VendorDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public async Task<VendorList> Create(IQueryable<Vendor> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new VendorList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }
        public async Task<int> GetCount(IQueryable<Vendor> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public async Task<List<VendorDto>> GetDtos(IQueryable<Vendor> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
               .Skip(skip)
               .AsNoTracking()
               .Select(c => new VendorDto
               {
                   Id = c.Id,
                   VendorName = c.VendorName,
                   Email = c.Email,
                   GSTNo= c.GSTNo,
                   Phone= c.Phone,
                   ResponsiblePersonName= c.ResponsiblePersonName,
                   VendorAddress= c.VendorAddress,
                   VendorCode= c.VendorCode,
                   Website = c.Website

               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               //.Take(pageSize)
               .AsNoTracking()
               .Select(c => new VendorDto
               {
                   Id = c.Id,
                   VendorName = c.VendorName,
                   Email = c.Email,
                   GSTNo = c.GSTNo,
                   Phone = c.Phone,
                   ResponsiblePersonName = c.ResponsiblePersonName,
                   VendorAddress = c.VendorAddress,
                   VendorCode = c.VendorCode,
                   Website = c.Website

               }).ToListAsync();
                return entities;
            }
        }
    }
}
