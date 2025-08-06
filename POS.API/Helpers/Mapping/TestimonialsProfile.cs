using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TestimonialsProfile'
    public class TestimonialsProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TestimonialsProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TestimonialsProfile.TestimonialsProfile()'
        public TestimonialsProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TestimonialsProfile.TestimonialsProfile()'
        {
            CreateMap<TestimonialsDto, Testimonials>().ReverseMap();
            CreateMap<AddTestimonialsCommand, Testimonials>();
            CreateMap<UpdateTestimonialsCommand, Testimonials>();
        }
    }
}
