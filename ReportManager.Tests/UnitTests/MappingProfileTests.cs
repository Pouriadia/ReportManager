using AutoMapper;
using ReportManager.API.Mapping;
using Xunit;

namespace ReportManager.Tests.UnitTests
{
    public class MappingProfileTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Act & Assert
            config.AssertConfigurationIsValid();
        }
    }
}