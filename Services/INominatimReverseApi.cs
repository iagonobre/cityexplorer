using Explorer.Models;
using Refit;

namespace Explorer.Services;

public interface INominatimReverseApi
{
    [Get("/reverse")]
    Task<NominatimReverseDto> ReverseAsync(
        [AliasAs("lat")] double latitude,
        [AliasAs("lon")] double longitude,
        [AliasAs("format")] string format
    );
}