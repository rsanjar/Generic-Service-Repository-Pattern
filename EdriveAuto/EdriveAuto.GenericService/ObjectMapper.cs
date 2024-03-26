using AutoMapper;
using EdriveAuto.DTO;
using EdriveAuto.GenericRepository;

namespace EdriveAuto.GenericService;

public static class ObjectMapper<TDTOModel, TRepositoryModel>
    where TDTOModel : class, IBaseDTOModel, new() 
    where TRepositoryModel : class, IBaseRepositoryModel, new()
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MapperConfiguration MapperConfiguration;
    public static IMapper Mapper => new Mapper(MapperConfiguration);

    static ObjectMapper()
    {
        MapperConfiguration ??= CreateMap();
    }

    private static MapperConfiguration CreateMap()
    {
        return new (cfg =>
        {
            cfg.CreateMap<TRepositoryModel, TDTOModel>(MemberList.Destination);
            cfg.CreateMap<TDTOModel, TRepositoryModel>(MemberList.Source);
        });
    }
}