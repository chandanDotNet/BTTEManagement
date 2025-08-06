using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.ApprovalLevel.Command;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelProfile'
    public class ApprovalLevelProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelProfile.ApprovalLevelProfile()'
        public ApprovalLevelProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ApprovalLevelProfile.ApprovalLevelProfile()'
        {
            CreateMap<ApprovalLevel, ApprovalLevelDto>().ReverseMap();
            CreateMap<AddApprovalLevelCommand, ApprovalLevel>().ReverseMap();
            CreateMap<ApprovalLevelUser, ApprovalLevelUserDto>().ReverseMap();
        }
    }
}
