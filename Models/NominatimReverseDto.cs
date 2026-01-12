using System.Text.Json.Serialization;

namespace Explorer.Models;

public class NominatimReverseDto
{
    [JsonPropertyName("address")]
    public NominatimAddressDto Address { get; set; } = new();
}

public class NominatimAddressDto
{
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("town")]
    public string? Town { get; set; }

    [JsonPropertyName("village")]
    public string? Village { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}