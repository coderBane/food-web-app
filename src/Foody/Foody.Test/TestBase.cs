using System;
using AutoMapper;
using FakeItEasy;
using Foody.Data.Services;
using Foody.Data.Interfaces;
using Foody.WebApi.Profiles;

namespace Foody.Test
{
    public class TestBase
    {
        public readonly IMapper mapper;
        public readonly IUnitofWork unitofWork = A.Fake<IUnitofWork>();
        public readonly ICacheService cacheService = A.Fake<ICacheService>();

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

