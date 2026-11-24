using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace GuildHub.Modules.Blizzard;

internal sealed class BlizzardTokenClient(HttpClient http, IOptions<BlizzardOptions> options)
{
   public async Task<TokenResponse> GetClientTokenAsync(CancellationToken ct = default)
   {
      var o = options.Value;
      var credentials = Convert.ToBase64String(
          Encoding.UTF8.GetBytes($"{o.ClientId}:{o.ClientSecret}"));

      using var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.battle.net/token");
      request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
      request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
      {
         ["grant_type"] = "client_credentials"
      });

      using var response = await http.SendAsync(request, ct);
      response.EnsureSuccessStatusCode();
      return (await response.Content.ReadFromJsonAsync<TokenResponse>(ct))!;
   }
}
