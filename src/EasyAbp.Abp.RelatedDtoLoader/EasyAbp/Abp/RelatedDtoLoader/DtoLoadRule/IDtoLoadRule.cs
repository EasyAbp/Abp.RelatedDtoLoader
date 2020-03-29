using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyAbp.Abp.RelatedDtoLoader.DtoLoadRule
{
    public interface IDtoLoadRule
    {
        object GetKey(object dto);
        Task<IEnumerable<object>> LoadAsObjectAsync(IServiceProvider serviceProvider, IEnumerable<object> ids);
    }
}