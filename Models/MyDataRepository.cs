// TODO: fix this class : class name : "RepositoryCaching" 
using system;

////public class MyDataRepository
{

        // TODO: check what happens when different users use Cache simultaneously
        public ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        public bool IsInMemory(string Key)
        {
            return Cache.Contains(Key);
        }

        public void Add(string Key, object Value, int Expiration)
        {
            Cache.Add(Key, Value, new CacheItemPolicy().AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(Expiration));
        }

        public IQueryable<T> FetchData<T>(string Key) where T : class
        {
            return Cache[Key] as IQueryable<T>;
        }

        public void Remove(string Key)
        {
            Cache.Remove(Key);
        }

    }
}
