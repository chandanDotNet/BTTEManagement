using AutoMapper;
using POS.Data;
using POS.Data.Dto;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberProfile'
    public class NewsletterSubscriberProfile:Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberProfile.NewsletterSubscriberProfile()'
        public NewsletterSubscriberProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberProfile.NewsletterSubscriberProfile()'
        {
            CreateMap<NewsletterSubscriber, NewsletterSubscriberDto>().ReverseMap();
        }
    }
}
