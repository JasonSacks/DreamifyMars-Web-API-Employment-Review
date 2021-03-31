using System.Text.Json.Serialization;

namespace DreamInMars.Dto
{
    public class AuthorizationRequest
    {
        [JsonPropertyName("tokenId")]
        public string TokenId { get; set; }
    }
}
