using System.Collections.Generic;

namespace Hiro2
{
    public interface IMap<TKey, TValue>
    {
        bool Contains(TKey key);

        IEnumerable<TKey> Keys { get; }

        void Add(TKey key, TValue item);
        TValue GetItem(TKey key);
    }
}