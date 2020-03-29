using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Shouldly;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class RelatedDtoLoaderConfigurationTest
    {
        private void Check_RelatedDtoPropertie_By_Profile(DtoLoaderConfiguration config)
        {
            config.GetRelatedDtoProperties(typeof(OrderDto)).ShouldNotBeNull();

            config.GetRelatedDtoProperties(typeof(UnsupportedOrderDto)).ShouldBeNull();
            config.GetRelatedDtoProperties(typeof(Order)).ShouldBeNull();
            config.GetRelatedDtoProperties(typeof(Product)).ShouldBeNull();

            config.GetRelatedDtoProperties(typeof(Product)).ShouldBeNull();

            config.GetRelatedDtoProperties(typeof(ProductDto)).ShouldBeNull();
        }

        private void Check_LoadRule_By_Profile(DtoLoaderConfiguration config)
        {
            config.GetLoadRule(typeof(OrderDto)).ShouldBeNull();

            config.GetLoadRule(typeof(ProductDto)).ShouldNotBeNull();
        }

        private static DtoLoaderConfiguration GetDtoLoaderConfiguration()
        {
            var configurationExpress = new DtoLoaderConfigurationExpression();
            configurationExpress.AddProfile(new MyUnitTestDtoLoaderProfile());

            var config = new DtoLoaderConfiguration(configurationExpress);
            return config;
        }

        [Fact]
        public void Should_Have_Correct_RelatedDtoProperties_By_Assembly()
        {
            var configurationExpress = new DtoLoaderConfigurationExpression();
            var assemblyOptions = new RelatedDtoLoaderAssemblyOptions();

            configurationExpress.AddAssemblies(assemblyOptions, typeof(RelatedDtoLoaderConfigurationTest).Assembly);
            var config = new DtoLoaderConfiguration(configurationExpress);

            Check_RelatedDtoPropertie_By_Profile(config);
            Check_LoadRule_By_Profile(config);
        }

        [Fact]
        public void Should_Have_Correct_RelatedDtoProperties_By_Profile()
        {
            var config = GetDtoLoaderConfiguration();

            Check_RelatedDtoPropertie_By_Profile(config);
            Check_LoadRule_By_Profile(config);
        }

        [Fact]
        public void Should_Have_Null_RelatedDtoProperty_When_Undefined()
        {
            var config = GetDtoLoaderConfiguration();

            var dtoProperties = config.GetRelatedDtoProperties(typeof(ProductDto));

            dtoProperties.ShouldBeNull();
        }
    }
}