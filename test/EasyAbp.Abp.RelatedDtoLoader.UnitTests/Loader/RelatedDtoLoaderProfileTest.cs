using EasyAbp.Abp.RelatedDtoLoader.Tests;
using Moq;
using Shouldly;
using System;
using System.Collections;
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

            profile.GetRelatedDtoProperties(typeof(OrderDto)).ShouldNotBeNull();
            profile.GetRelatedDtoProperties(typeof(ProductDto)).ShouldNotBeNull();

            profile.GetRelatedDtoProperties(typeof(UnsupportedOrderDto)).ShouldBeNull();
            profile.GetRelatedDtoProperties(typeof(Order)).ShouldBeNull();
            profile.GetRelatedDtoProperties(typeof(Product)).ShouldBeNull();

            profile.GetRelatedDtoProperties(typeof(Product)).ShouldBeNull();
        }

        [Fact]
        public void Should_Have_Correct_RelatedDtoProperty()
        {
            var profile = GetRelatedDtoProfile();

            var dtoProperties = profile.GetRelatedDtoProperties(typeof(ProductDto));
            var commentsProperty = dtoProperties.FirstOrDefault();
            var commentsDtoType = commentsProperty.DtoType;

            commentsDtoType.ElementType.ShouldBe(typeof(ProductCommentDto));
            commentsDtoType.GenericType.ShouldBe(typeof(IEnumerable<>));
            commentsDtoType.Property.PropertyType.ShouldBe(typeof(IEnumerable<ProductCommentDto>));
        }

        private static MyUnitTestDtoLoaderProfile GetRelatedDtoProfile()
        {
            var profile = new MyUnitTestDtoLoaderProfile();
            return profile;
        }
    }
}
