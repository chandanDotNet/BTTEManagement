using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Data;
using POS.Data.Dto;
using AutoMapper;

namespace POS.Repository
{
    public class UserList : List<UserDto>
    {
        //public UserList()
        //{
        //}

        private readonly IUserRoleRepository _userRoleRepository;
        public UserList(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public UserList(List<UserDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<UserList> Create(IQueryable<User> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new UserList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<User> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        private async Task<List<RoleDto>> GetUserRole(Guid UserId)
        {
            var rolesDetails = await _userRoleRepository.AllIncluding(c => c.Role).Where(d => d.UserId == UserId)
                .ToListAsync();

            List<RoleDto> roleDto = new List<RoleDto>();
            foreach (var role in rolesDetails)
            {
                RoleDto rd = new RoleDto();
                rd.Id = role.Role.Id;
                rd.Name = role.Role.Name;
                roleDto.Add(rd);
            }
            // var roleClaims = await _roleClaimRepository.All.Where(c => rolesIds.Contains(c.RoleId)).Select(c => c.ClaimType).ToListAsync();
            return roleDto;
        }

        public async Task<List<UserDto>> GetDtos(IQueryable<User> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new UserDto
                {
                    Id = c.Id,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    IsActive = c.IsActive,
                    Designation = c.Designation,
                    Grade = c.Grade,
                    Department = c.Department,
                    AadhaarNo = c.AadhaarNo,
                    Address = c.Address,
                    DateOfBirth = c.DateOfBirth,
                    DateOfJoining = c.DateOfJoining,
                    EmployeeCode = c.EmployeeCode,
                    PanNo = c.PanNo,
                    ProfilePhoto = c.ProfilePhoto,
                    //Roles = _userRoleRepository.AllIncluding(r => r.Role).Where(r => r.UserId == c.Id).Select(a=>a.Role.Name).ToList(),
                    Roles= GetUserRole(c.Id).Result

                })
                .ToListAsync();
            return entities;
        }
    }
}
