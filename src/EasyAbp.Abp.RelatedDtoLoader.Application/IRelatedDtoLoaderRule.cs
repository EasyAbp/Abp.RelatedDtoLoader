using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public interface IRelatedDtoLoaderRule 
    {
        object GetKey(object dto);
        Task<IEnumerable<object>> LoadAsObjectAsync(IEnumerable<object> ids);
    }
}