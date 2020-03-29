using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoader
    {
        Task<IEnumerable<TTargetDto>> LoadListAsync<TTargetDto, TKeyProvider>(IEnumerable<TTargetDto> targetDtos,
            IEnumerable<TKeyProvider> keyProviders)
            where TTargetDto : class
            where TKeyProvider : class;
    }
}