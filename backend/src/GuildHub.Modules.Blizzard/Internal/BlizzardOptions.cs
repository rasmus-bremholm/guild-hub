using System.Reflection.Metadata;
using System.Text.Json.Serialization;

internal sealed class BlizzardOptions
{
   public string ClientId { get; set; } = "";
   public string ClientSecret { get; set; } = "";
   public string Region { get; set; } = "eu";

}

internal sealed record TokenResponse(
   [property: JsonPropertyName("access_token")] string AccessToken,
   [property: JsonPropertyName("expires_in")] int ExpiresIn
);
