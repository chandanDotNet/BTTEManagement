using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Branch.Command;
using BTTEM.MediatR.ApprovalLevel.Command;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelTypeProfile'
    public class ApprovalLevelTypeProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelTypeProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelTypeProfile.ApprovalLevelTypeProfile()'
        public ApprovalLevelTypeProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelTypeProfile.ApprovalLevelTypeProfile()'
        {
            CreateMap<ApprovalLevelType, ApprovalLevelTypeDto>().ReverseMap();
            CreateMap<AddApprovalLevelTypeCommand, ApprovalLevelType>().ReverseMap();
        }
    }
}
