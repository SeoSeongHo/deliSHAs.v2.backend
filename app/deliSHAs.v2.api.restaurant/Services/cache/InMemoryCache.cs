using deliSHAs.v2.api.restaurant.Models.exception;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Services.cache
{
    public class InMemoryCache
    {
        public MemoryCache _cache;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public InMemoryCache(int sizeLimit = 1024)
        {
            _cache = new MemoryCache
            (
                new MemoryCacheOptions
                {
                    SizeLimit = sizeLimit
                }
            );
        }

        /// <summary>
        /// Cache GetOrCreate Method
        /// Cache 에 key 에 대한 entry 가 있다면, 해당 값 return 해주는 메소드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (!_cache.TryGetValue(key, out CacheEntry cacheEntry))
                return (T)default;

            return (T)cacheEntry.data;
        }

        /// <summary>
        /// cahce 에 새로운 값을 생성하는 메소드
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public async Task<bool> Create<T>(string key, T entry)
        {
            bool isCreated = false;

            await _semaphore.WaitAsync();

            try
            {
                if (!_cache.TryGetValue(key, out CacheEntry cacheEntry))
                {
                    cacheEntry = new CacheEntry
                    {
                        data = entry,
                        absoluteExpirationDateTime = DateTime.Now.AddDays(2)
                    };

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSize(1)            
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSlidingExpiration(TimeSpan.FromDays(3))       // entry 에 해당 시간동안 접근이 없다면 제거 됨
                            .SetAbsoluteExpiration(TimeSpan.FromDays(3));    // entry 가 캐시될 수 있는 최대 시간

                    cacheEntry = _cache.Set(key, cacheEntry, cacheEntryOptions);

                    isCreated = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception : {e.Message}");
            }
            finally
            {
                _semaphore.Release();
            }

            return isCreated;
        }


        /// <summary>
        /// Cache Update Method
        /// 사실상 잘 쓰이진 않겠지만, 디버깅이나 긴급보수 용도로 만들어 놓음.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="Entry"></param>
        /// <returns></returns>
        public async Task<bool> Update<T>(string key, T entry)
        {
            bool isUpdated = false;

            await _semaphore.WaitAsync();

            try
            {
                if(_cache.TryGetValue(key, out CacheEntry cacheEntry))
                {
                    cacheEntry = new CacheEntry
                    {
                        data = entry,
                        absoluteExpirationDateTime = DateTime.Now.AddDays(2)
                    };

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSize(1)
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSlidingExpiration(TimeSpan.FromDays(3))       // entry 에 해당 시간동안 접근이 없다면 제거 됨
                            .SetAbsoluteExpiration(TimeSpan.FromDays(3));    // entry 가 캐시될 수 있는 최대 시간

                    cacheEntry = _cache.Set(key, cacheEntry, cacheEntryOptions);

                    isUpdated = true;
                }


            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception : {e.Message}");
            }
            finally
            {
                _semaphore.Release();
            }

            return isUpdated;
        }
    }


    public class CacheEntry
    {
        public object data;
        public DateTime absoluteExpirationDateTime;
    }
}
