using Refit;
using Explorer.Models;

namespace Explorer.Services;

public interface INominatimApi
{
    [Get("/search")]
    Task<List<NominatimPlaceDto>> SearchAsync(
        [AliasAs("q")] string query,
        [AliasAs("format")] string format,
        [AliasAs("limit")] int limit,
        [AliasAs("lat")] double latitude,
        [AliasAs("lon")] double longitude
    );
}