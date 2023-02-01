using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public class ActiveDirectoryDistributedTokenCache : IActiveDirectoryTokenCache
    {
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public ActiveDirectoryDistributedTokenCache(IDistributedCache distributedCache, IOptions<DistributedCacheEntryOptions> options)
        {
            ArgumentNullException.ThrowIfNull(distributedCache, nameof(distributedCache));
            ArgumentNullException.ThrowIfNull(options.Value, nameof(options));
            _distributedCache = distributedCache;
            _cacheOptions = options.Value;
        }

        public void EnableSerialization(ITokenCache tokenCache)
        {
            tokenCache.SetBeforeAccess(BeforeAccessNotification);
            tokenCache.SetAfterAccess(AfterAccessNotification);
        }

        internal void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            var cacheKey = BuildCacheKey(args.ClientId);
            var cachedItem = _distributedCache.Get(cacheKey);
            if (cachedItem != null)
            {
                args.TokenCache.DeserializeMsalV3(cachedItem);
            }
        }

        internal void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (args.HasStateChanged)
            {
                var cacheKey = BuildCacheKey(args.ClientId);
                var cacheItem = args.TokenCache.SerializeMsalV3();
                _distributedCache.Set(cacheKey, cacheItem, _cacheOptions);
            }
        }

        private static string BuildCacheKey(string clientId) => $"clientid:{clientId}";
    }
}
