using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.Branch.Command;

namespace BTTEM.API.Helpers.Mapping
{
    public class CostCenterProfile : Profile
    {
        public CostCenterProfile()
        {
            CreateMap<CostCenter, CostCenterDto>().ReverseMap();
        }
    }
}
