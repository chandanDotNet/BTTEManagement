﻿using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class CityList : List<CityDto>
    {
        public CityList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public CityList(List<CityDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CityList> Create(IQueryable<City> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new CityList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<City> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<CityDto>> GetDtos(IQueryable<City> source, int skip, int pageSize)
        {

            if(pageSize==0)
            {
                var entities = await source
               .Skip(skip)              
               .AsNoTracking()
               .Select(c => new CityDto
               {
                   Id = c.Id,
                   CityName = c.CityName,
                   CountryId = c.Country.Id,
                   CountryName = c.Country.CountryName
               }).ToListAsync();
                return entities;
            }
            else
            {
                var entities = await source
               .Skip(skip)
               .Take(pageSize)
               .AsNoTracking()
               .Select(c => new CityDto
               {
                   Id = c.Id,
                   CityName = c.CityName,
                   CountryId = c.Country.Id,
                   CountryName = c.Country.CountryName
               }).ToListAsync();
                return entities;
            }
           
           
        }
    }
}
