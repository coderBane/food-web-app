using AutoMapper;
using Foody.WebApi.Profiles;

namespace Foody.Test.Systems.AutoMapper
{
    public class ConfigurationTests
    {
        [Fact]
        public void AutoMapper_ItemProfile_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ItemProfile>());
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void AutoMapper_CommunicationProfile_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CommunicationProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}

