using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Branch.Command;

namespace BTTEM.API.Helpers.Mapping
{
    public class BranchProfile :Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDto>().ReverseMap();
            CreateMap<AddBranchCommand, Branch>().ReverseMap();
        }
    }
}
