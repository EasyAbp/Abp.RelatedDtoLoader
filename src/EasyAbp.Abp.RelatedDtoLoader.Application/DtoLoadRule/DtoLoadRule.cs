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
        public DtoLoadRule(Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> rule)
            : this()
        {
            Rule = rule;
        }

        protected DtoLoadRule()
        {
        }

        protected Func<IServiceProvider, IEnumerable<TKey>, Task<IEnumerable<TDto>>> Rule { get; set; }

        public async Task<IEnumerable<object>> LoadAsObjectAsync(IServiceProvider serviceProvider,
            IEnumerable<object> ids)
        {
            var convertedIds = ids.Select(x => (TKey) x);

            return (await Rule(serviceProvider, convertedIds)).AsEnumerable<object>().ToArray();
        }

        public object GetKey(object dto)
        {
            return ((TDto) dto).Id;
        }

        public async Task<IEnumerable<TDto>> LoadAsync(IServiceProvider serviceProvider, IEnumerable<TKey> ids)
        {
            return await Rule(serviceProvider, ids);
        }
    }
}