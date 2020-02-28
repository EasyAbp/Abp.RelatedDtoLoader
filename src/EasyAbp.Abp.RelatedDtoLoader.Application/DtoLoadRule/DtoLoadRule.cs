using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class DtoLoadRule<TDto, TKey> : IDtoLoadRule
        where TDto : class, IEntityDto<TKey>
    {        
        protected Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> Rule { get; set; }

        public DtoLoadRule(Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> rule)
           : this()
        {
            Rule = rule;
        }

        protected DtoLoadRule()
        {
        }

        public async Task<IEnumerable<TDto>> LoadAsync(IEnumerable<TKey> ids)
        {
            return await Rule(ids).ConfigureAwait(false);
        }

        public async Task<IEnumerable<object>> LoadAsObjectAsync(IEnumerable<object> ids)
        {
            var convertedIds = ids.Select(x => (TKey)x);
            
            return (await Rule(convertedIds).ConfigureAwait(false)).AsEnumerable<object>().ToArray();
        }

        public object GetKey(object dto)
        {
            return ((TDto)dto).Id;
        }
    }
}