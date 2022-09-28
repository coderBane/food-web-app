using System;
using AutoMapper;
using FakeItEasy;
using Foody.Data.Interfaces;
using Foody.WebApi.Profiles;

namespace Foody.Test
{
    public class TestBase
    {
        public readonly IUnitofWork unitofWork = A.Fake<IUnitofWork>();
        public readonly IMapper mapper;

        public TestBase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(new List<Profile>
                {
                    new ItemProfile(),
                });
            });
            mapper = config.CreateMapper();
        }
    }
}

