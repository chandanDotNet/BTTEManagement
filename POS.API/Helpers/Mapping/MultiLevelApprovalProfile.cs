using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class MultiLevelApprovalProfile : Profile
    {
        public MultiLevelApprovalProfile()
        {
            CreateMap<MultiLevelApproval, MultiLevelApprovalDto>().ReverseMap();
            CreateMap<AddMultiLevelApprovalCommand, MultiLevelApproval>();
            CreateMap<UpdateMultiLevelApprovalCommand, MultiLevelApproval>();
        }
    }
}
