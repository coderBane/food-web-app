using AutoMapper;
using Foody.Entities.DTOs;
using Foody.Entities.Models;

namespace Foody.WebApi.Profiles;

public class CommunicationProfile : Profile
{
    public CommunicationProfile()
    {
        CreateMap<NewsletterDto, Newsletter>()
            .ForMember(src => src.Id, opt => opt.Ignore());
    }
}

