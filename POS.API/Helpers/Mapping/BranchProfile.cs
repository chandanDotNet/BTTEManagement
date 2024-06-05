using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class BranchProfile :Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDto>().ReverseMap();
        }
    }
}
