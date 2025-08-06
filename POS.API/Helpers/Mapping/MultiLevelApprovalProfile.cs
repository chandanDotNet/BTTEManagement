using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalProfile'
    public class MultiLevelApprovalProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalProfile.MultiLevelApprovalProfile()'
        public MultiLevelApprovalProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalProfile.MultiLevelApprovalProfile()'
        {
            CreateMap<MultiLevelApproval, MultiLevelApprovalDto>().ReverseMap();
            CreateMap<AddMultiLevelApprovalCommand, MultiLevelApproval>();
            CreateMap<UpdateMultiLevelApprovalCommand, MultiLevelApproval>();
        }
    }
}
