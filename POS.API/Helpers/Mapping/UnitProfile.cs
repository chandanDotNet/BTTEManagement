using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.UnitConversation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnitProfile'
    public class UnitProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnitProfile'
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnitProfile()
        {

            CreateMap<UnitConversation, UnitConversationDto>().ReverseMap();
            CreateMap<AddUnitConversationCommand, UnitConversation>();
            CreateMap<UpdateUnitConversationCommand, UnitConversation>();
        }
    }
}
