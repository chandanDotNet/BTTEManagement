using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Department.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DepartmentProfile'
    public class DepartmentProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DepartmentProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DepartmentProfile.DepartmentProfile()'
        public DepartmentProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DepartmentProfile.DepartmentProfile()'
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<AddDepartmentCommand, Department>();
            CreateMap<UpdateDepartmentCommand, Department>();
        }
    }
}
