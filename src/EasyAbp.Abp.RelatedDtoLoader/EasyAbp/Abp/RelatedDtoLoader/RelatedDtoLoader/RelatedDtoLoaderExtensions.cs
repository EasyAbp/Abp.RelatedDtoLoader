using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAbp.Abp.RelatedDtoLoader.RelatedDtoLoader
{
    public static class RelatedDtoLoaderExtensions
    {
        public static async Task<TTargetDto> LoadAsync<TTargetDto>(this IRelatedDtoLoader loader, TTargetDto targetDto)
            where TTargetDto : class
        {
            return (await loader.LoadListAsync(new[] {targetDto})).First();
        }

        public static async Task<TTargetDto> LoadAsync<TTargetDto, TKeyProvider>(this IRelatedDtoLoader loader,
            TTargetDto targetDto, TKeyProvider keyProvider)
            where TTargetDto : class
            where TKeyProvider : class
        {
            return (await loader.LoadListAsync(new[] {targetDto}, new[] {keyProvider})).First();
        }

        public static async Task<IEnumerable<TTargetDto>> LoadListAsync<TTargetDto>(this IRelatedDtoLoader loader,
            IEnumerable<TTargetDto> targetDtos)
            where TTargetDto : class
        {
            var arrTargetDtos = targetDtos.ToArray();
            return await loader.LoadListAsync(arrTargetDtos, arrTargetDtos);
        }
    }
}