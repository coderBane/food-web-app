using AutoMapper;

namespace Foody.WebApi.Profiles;

public class CommunicationProfile : Profile
{
    public CommunicationProfile()
    {
        AddGlobalIgnore("Id");

        CreateMap<NewsletterDto, Newsletter>();

        CreateMap<ContactDto, Contact>()
            .ForMember(dest => dest.Date, opt => opt.Ignore())
            .ForMember(dest => dest.Read, opt => opt.Ignore());        
    }
}

