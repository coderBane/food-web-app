using AutoMapper;
using Foody.Data.Services;
using Foody.Entities.Repositories;
using Foody.WebApi.Profiles;


namespace Foody.Test
{
    public class TestBase
    {
        internal readonly IMapper mapper;
        internal readonly IUnitOfWork unitofWork = A.Fake<IUnitOfWork>();
        internal readonly ICacheService cacheService = A.Fake<ICacheService>();

        public TestBase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(new List<Profile>
                {
                    new ItemProfile(),
                    new CommunicationProfile(),
                });
            });
            mapper = config.CreateMapper();
        }
    }
}

