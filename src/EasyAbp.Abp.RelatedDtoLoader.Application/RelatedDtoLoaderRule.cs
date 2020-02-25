using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace EasyAbp.Abp.RelatedDtoLoader
{
    public class RelatedDtoLoaderRule<TDto, TKey> : IRelatedDtoLoaderRule<TDto, TKey> 
        where TDto : class, IEntityDto
    {        
        protected Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> Rule { get; set; }
        
        protected RelatedDtoLoaderRule() { }

        public RelatedDtoLoaderRule(Func<IEnumerable<TKey>, Task<IEnumerable<TDto>>> rule)
        {
            Rule = rule;
        }
      
        public async Task<TDto> LoadDtoAsync(TKey id)
        {
            return (await LoadDtosAsync(new TKey[] {id})).First();
        }
        
        public async Task<IEnumerable<TDto>> LoadDtosAsync(IEnumerable<TKey> ids)
        {
            return await Rule(ids);
        }

        public async Task<IEnumerable<object>> LoadDtoObjectsAsync(IEnumerable<object> ids)
        {
            var convertedIds = ids.Select(x => (TKey)x);
            return (await Rule(convertedIds).ConfigureAwait(false)).AsEnumerable<object>().ToArray();
        }
    }
}