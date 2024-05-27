using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.Service.Internal.Contract
{
    public interface ICacheService
    {
        TModel? Get<TModel>(object key);
        void Set<TModel>(object key, TModel value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CachePriority? cachePriority = null);
        void Remove(object key);
    }

    public enum CachePriority
    {
        Low,
        Normal,
        High,
        NeverRemove
    }
}
