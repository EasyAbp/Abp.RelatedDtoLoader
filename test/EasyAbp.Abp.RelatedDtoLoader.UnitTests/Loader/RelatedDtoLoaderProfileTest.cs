using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class RelatedDtoLoaderProfileTest
    {        
        [Fact]
        public void Should_Have_Null_RelatedDtoProperties_for_Unsupported_TargetDtoType()
        {
            var profile = GetRelatedDtoProfile();

            profile.GetRelatedDtoProperties(typeof(OrderDto));
            profile.GetRelatedDtoProperties(typeof(ProductDto)).ShouldBeNull();
            profile.GetRelatedDtoProperties(typeof(UnsupportedOrderDto)).ShouldBeNull();
            profile.GetRelatedDtoProperties(typeof(Order)).ShouldBeNull();
            profile.GetRelatedDtoProperties(typeof(Product)).ShouldBeNull();

            profile.UnsupportedTargetDtoTypeCount.ShouldBe(MyUnitTestDtoLoaderProfile.TestInvalidTargetDtoTypeCacheCount);

            profile.GetRelatedDtoProperties(typeof(Product)).ShouldBeNull();
        }

        private static MyUnitTestDtoLoaderProfile GetRelatedDtoProfile()
        {
            var profile = new MyUnitTestDtoLoaderProfile(null);
            return profile;
        }
    }
}
