using Newtonsoft.Json;

namespace EdriveAuto.GenericPagination;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public interface IBasePagination
{
    [JsonProperty]
    int PageSize { get; set; }

    [JsonProperty]
    int PageNumber { get; set; }

    [JsonProperty]
    int TotalCount { get; }

    [JsonProperty]
    string? OrderBy { get; set; }

    [JsonProperty]
    bool? IsOrderAscending { get; set; }

    [JsonProperty]
    int TotalPages { get; }

    [JsonProperty]
    bool HasNextPage { get; }

    [JsonProperty]
    bool HasPreviousPage { get; }

    [JsonProperty]
    bool IsFirstPage { get; }

    [JsonProperty]
    bool IsLastPage { get; }
}