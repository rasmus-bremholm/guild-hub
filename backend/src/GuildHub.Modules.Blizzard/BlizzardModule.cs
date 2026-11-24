using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace GuildHub.Modules.Blizzard;

public static class BlizzardModule
{
   public static IServiceCollection AddBlizzardModule(
      this IServiceCollection services,
      IConfiguration configuration
   )
   {
      services.Configure<BlizzardOptions>(configuration.GetSection("Blizzard"));
      services.AddHttpClient<BlizzardTokenClient>();
      return services;
   }


   public static IEndpointRouteBuilder MapBlizzardEndpoints(this IEndpointRouteBuilder app)
   {
      app.MapGet("/blizzard/config-check", (IOptions<BlizzardOptions> options) =>
      {
         var o = options.Value;
         return new
         {
            HasClientId = !string.IsNullOrEmpty(o.ClientId),
            HasClientSecret = !string.IsNullOrEmpty(o.ClientSecret),
            o.Region
         };
      });


      app.MapGet("/blizzard/token-check", async (BlizzardTokenClient tokens, CancellationToken ct) =>
      {
         var token = await tokens.GetClientTokenAsync(ct);
         return new
         {
            GotToken = !string.IsNullOrEmpty(token.AccessToken),
            token.ExpiresIn
         };
      });
      return app;
   }
}
