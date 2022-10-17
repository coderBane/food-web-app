using AutoMapper;
using Foody.WebApi.Profiles;

namespace Foody.Test.Systems.AutoMapper
{
    public class ConfigurationTests
    {
        [Theory]
        [InlineData(typeof(ItemProfile))]
        [InlineData(typeof(CommunicationProfile))]
        public void AutoMapper_Configuration_IsValid(Type profiles)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profiles));
            config.AssertConfigurationIsValid();
        }
    }
}

