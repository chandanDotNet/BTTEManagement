﻿using AutoMapper;
using BTTEM.Data;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.TravelDocument.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class TravelDocumentProfile : Profile
    {

        public TravelDocumentProfile()
        {
            CreateMap<TravelDocument, TravelDocumentDto>().ReverseMap();
            CreateMap<AddTravelDocumentCommand, TravelDocument>();
            CreateMap<UpdateTravelDocumentCommand, TravelDocument>();
            CreateMap<UpdateTravelDocumentStatusCommand, TravelDocument>();
        }
    }
}
