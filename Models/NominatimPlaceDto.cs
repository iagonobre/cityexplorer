using System.Text.Json.Serialization;

namespace Explorer.Models;

public class NominatimPlaceDto
{
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("lat")]
    public string Latitude { get; set; } = string.Empty;

    [JsonPropertyName("lon")]
    public string Longitude { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}