using Microsoft.Extensions.Caching.Memory;

namespace GuildHub.Modules.Blizzard;

internal sealed class BlizzardTokenProvider
{
   private const string CacheKey = "blizzard:client-token";

   private readonly BlizzardTokenClient _tokenClient;
   private readonly IMemoryCache _cache;

   public BlizzardTokenProvider(BlizzardTokenClient tokenClient, IMemoryCache cache)
   {
      _tokenClient = tokenClient;
      _cache = cache;
   }

   public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
   {
      if (_cache.TryGetValue(CacheKey, out string? cachedToken) && cachedToken is not null)
      {
         return cachedToken;
      }

      TokenResponse token = await _tokenClient.GetClientTokenAsync(ct);

      TimeSpan lifetime = TimeSpan.FromSeconds(token.ExpiresIn) - TimeSpan.FromMinutes(5);

      _cache.Set(CacheKey, token.AccessToken, lifetime);

      return token.AccessToken;
   }
}
