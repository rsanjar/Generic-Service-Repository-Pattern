using EdriveAuto.Common;
using Newtonsoft.Json;

namespace EdriveAuto.GenericPagination;

public interface IPaginatedList<T> : IBasePagination where T : IBaseModel
{
    [JsonProperty] List<T> ResultSet { get; }
}