using AutoMapper;

namespace Foody.WebApi.Profiles;

public class CommunicationProfile : Profile
{
    public CommunicationProfile()
    {
        this.AddGlobalIgnore("Id");

        CreateMap<NewsletterDto, Newsletter>();

        CreateMap<ContactDto, Contact>().
            ForMember(dest => dest.Date, opt => opt.Ignore());          
    }
}

