using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using EdriveAuto.DTO;
using EdriveAuto.GenericPagination;
using EdriveAuto.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace EdriveAuto.GenericService;

internal static class AutoMapperExtensions
{
    public static IQueryable<TDTOModel> ProjectTo<TRepositoryModel, TDTOModel>(this IQueryable<TRepositoryModel> query)
        where TRepositoryModel : class, IBaseRepositoryModel, new()
        where TDTOModel : class, IBaseDTOModel, new()
    {
        return query.ProjectTo<TDTOModel>(ObjectMapper<TDTOModel, TRepositoryModel>.Mapper.ConfigurationProvider);
    }

    public static TDTOModel Map<TRepositoryModel, TDTOModel>(this TRepositoryModel t)
        where TRepositoryModel : class, IBaseRepositoryModel, new()
        where TDTOModel : class, IBaseDTOModel, new()
    {
        return ObjectMapper<TDTOModel, TRepositoryModel>.Mapper.Map<TDTOModel>(t);
    }

    public static async Task<TDTOModel?> FirstOrDefaultAsync<TRepositoryModel, TDTOModel>(this IQueryable<TRepositoryModel> query,
        Expression<Func<TDTOModel?, bool>>? predicate = null)
        where TRepositoryModel : class, IBaseRepositoryModel, new()
        where TDTOModel : class, IBaseDTOModel, new()
    {
        IQueryable<TDTOModel?> baseModels = query.Select(c => c.Map<TRepositoryModel, TDTOModel>());

        if(predicate != null)
            return await baseModels.Where(predicate).FirstOrDefaultAsync();
            
        return await baseModels.FirstOrDefaultAsync();
    }

    public static async Task<List<TDTOModel>> ToListAsync<TRepositoryModel, TDTOModel>(this IQueryable<TRepositoryModel> query)
        where TRepositoryModel : class, IBaseRepositoryModel, new()
        where TDTOModel : class, IBaseDTOModel, new()
    {
        return await query.Select(c => c.Map<TRepositoryModel, TDTOModel>()).ToListAsync();
    }

    public static async Task<PaginatedList<TDTOModel>> GetPaginatedListAsync<TRepositoryModel, TDTOModel>(
	    this IQueryable<TRepositoryModel> source, PaginatedList<TDTOModel> pagination)
	    where TRepositoryModel : class, IBaseRepositoryModel, new()
	    where TDTOModel : class, IBaseDTOModel, new()
    {
	    var items = await source.ProjectTo<TRepositoryModel, TDTOModel>().PaginateAsync(pagination);

	    return new PaginatedList<TDTOModel>(items, pagination);
    }

    public static async Task<PaginatedList<TDTOModel>> GetPaginatedListAsync<TDTOModel>(
	    this IQueryable<TDTOModel> source, PaginatedList<TDTOModel> pagination) 
	    where TDTOModel : IBaseDTOModel
    {
	    var items = await source.PaginateAsync(pagination);

	    return new PaginatedList<TDTOModel>(items, pagination);
    }
}