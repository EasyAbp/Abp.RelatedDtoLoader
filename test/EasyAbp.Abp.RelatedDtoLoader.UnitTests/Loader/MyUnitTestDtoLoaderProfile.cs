using EasyAbp.Abp.RelatedDtoLoader.Tests;
using System;
using System.Linq;

namespace EasyAbp.Abp.RelatedDtoLoader.UnitTests
{
    public class MyUnitTestDtoLoaderProfile : RelatedDtoLoaderProfile
    {
        public const int TestInvalidTargetDtoTypeCacheCount = 2;

        public MyUnitTestDtoLoaderProfile(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InvalidTargetDtoTypeCacheCount = TestInvalidTargetDtoTypeCacheCount;

            CreateRule<ProductDto, Product>();
        }

        public int UnsupportedTargetDtoTypeCount
        {
            get { return _unsupportedTargetDtoTypes.Count; }
        }
    }
}
