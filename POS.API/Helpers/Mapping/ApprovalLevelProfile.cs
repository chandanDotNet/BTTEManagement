using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.ApprovalLevel.Command;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class ApprovalLevelProfile : Profile
    {
        public ApprovalLevelProfile()
        {
            CreateMap<ApprovalLevel, ApprovalLevelDto>().ReverseMap();
            CreateMap<AddApprovalLevelCommand, ApprovalLevel>().ReverseMap();
            CreateMap<ApprovalLevelUser, ApprovalLevelUserDto>().ReverseMap();
        }
    }
}
