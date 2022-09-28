using AutoMapper;
using Foody.WebApi.Profiles;

namespace Foody.Test.AutoMapper
{
    public class ConfigurationTests
    {
        [Fact]
        public void AutoMapper_ItemProfile_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ItemProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}

