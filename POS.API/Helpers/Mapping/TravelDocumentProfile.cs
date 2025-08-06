using AutoMapper;
using BTTEM.Data;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.TravelDocument.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TravelDocumentProfile'
    public class TravelDocumentProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TravelDocumentProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TravelDocumentProfile.TravelDocumentProfile()'
        public TravelDocumentProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TravelDocumentProfile.TravelDocumentProfile()'
        {
            CreateMap<TravelDocument, TravelDocumentDto>().ReverseMap();
            CreateMap<AddTravelDocumentCommand, TravelDocument>();
            CreateMap<UpdateTravelDocumentCommand, TravelDocument>();
            CreateMap<UpdateTravelDocumentStatusCommand, TravelDocument>();
        }
    }
}
