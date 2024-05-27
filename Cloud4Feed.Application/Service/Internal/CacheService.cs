using Cloud4Feed.Application.Service.Internal.Contract;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.Service.Internal
{
    public class CacheService : ICacheService
    {
        MemoryCacheEntryOptions defaultEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetPriority(CacheItemPriority.Normal);

        readonly IMemoryCache cache;

        public CacheService(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public TModel? Get<TModel>(object key)
        {
            if (cache.TryGetValue(key, out TModel? value))
                return value;
            return default;
        }

        public void Set<TModel>(object key, TModel value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CachePriority? cachePriority = null)
        {

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? defaultEntryOptions.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = slidingExpiration ?? defaultEntryOptions.SlidingExpiration,
                Priority = MatchPriority(cachePriority)
            };

            cache.Set(key, value, options);
        }

        private CacheItemPriority MatchPriority(CachePriority? priority)
        {
            return priority switch
            {
                CachePriority.Low => CacheItemPriority.Low,
                CachePriority.Normal => CacheItemPriority.Normal,
                CachePriority.High => CacheItemPriority.High ,
                CachePriority.NeverRemove => CacheItemPriority.NeverRemove,
                _ => CacheItemPriority.Normal
            };
        }

        public void Remove(object key)
        {
            cache.Remove(key);
        }
    }
}
