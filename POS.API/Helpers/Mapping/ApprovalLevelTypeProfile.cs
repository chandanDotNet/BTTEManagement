using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Branch.Command;
using BTTEM.MediatR.ApprovalLevel.Command;

namespace BTTEM.API.Helpers.Mapping
{
    public class ApprovalLevelTypeProfile : Profile
    {
        public ApprovalLevelTypeProfile()
        {
            CreateMap<ApprovalLevelType, ApprovalLevelTypeDto>().ReverseMap();
            CreateMap<AddApprovalLevelTypeCommand, ApprovalLevelType>().ReverseMap();
        }
    }
}
