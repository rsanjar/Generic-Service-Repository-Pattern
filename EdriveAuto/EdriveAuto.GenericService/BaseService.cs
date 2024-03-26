using EdriveAuto.Common;
using EdriveAuto.DTO;
using EdriveAuto.GenericPagination;
using EdriveAuto.GenericRepository;
using EdriveAuto.GenericService.Interfaces;

namespace EdriveAuto.GenericService;

public class BaseService<TDTOModel, TRepositoryModel> : IBaseService<TDTOModel>
    where TDTOModel : class, IBaseDTOModel, new()
    where TRepositoryModel : class, IBaseRepositoryModel, new()
{
    private readonly IGenericRepository<TRepositoryModel> _repository;

    public BaseService(IGenericRepository<TRepositoryModel> repository)
    {
        _repository = repository;
    }

    public virtual async Task<PaginatedList<TDTOModel>> GetAllAsync(PaginatedList<TDTOModel> pagination)
    {
        return await _repository.Entity.GetPaginatedListAsync(pagination);
    }

    public virtual async Task<PaginatedList<TDTOModel>> GetAllAsync(int pageNumber, int pageSize, string orderByString, bool isAsc = true)
    {
        return await _repository.Entity.GetPaginatedListAsync(new PaginatedList<TDTOModel>(orderByString, pageNumber, pageSize, isAsc));
    }

    public virtual async Task<TDTOModel?> GetAsync(int id)
    {
        var result = await _repository.GetAsync(id);
        return (result)?.Map<TRepositoryModel, TDTOModel>();
    }

    public virtual async Task<CrudResponse> SaveAsync(TDTOModel item)
    {
        var newItem = Convert(item);
        var result = await _repository.SaveAsync(newItem);
        item.ID = newItem.ID;
        return result;
    }

    public virtual async Task<CrudResponse> SaveAllAsync(List<TDTOModel> list)
    {
        var newList = list.Select(Convert).ToList();
        return await _repository.SaveAllAsync(newList);
    }

    public virtual async Task<CrudResponse> UpdateAsync(TDTOModel item)
    {
        return await _repository.UpdateAsync(Convert(item));
    }

    public virtual async Task<CrudResponse> SaveOrUpdateAsync(TDTOModel item)
    {
        var newItem = Convert(item);
        var result = await _repository.SaveOrUpdateAsync(newItem);
        item.ID = newItem.ID;
        return result;
    }

    public virtual async Task<CrudResponse> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    protected static async Task<TDTOModel?> FirstOrDefaultAsync(IQueryable<TRepositoryModel> t)
    {
        return await t.FirstOrDefaultAsync<TRepositoryModel, TDTOModel>();
    }

    protected static async Task<List<TDTOModel>> ToListAsync(IQueryable<TRepositoryModel> t)
    {
        return await t.ToListAsync<TRepositoryModel, TDTOModel>();
    }

    protected static TRepositoryModel Convert(TDTOModel t)
    {
        return ObjectMapper<TDTOModel, TRepositoryModel>.Mapper.Map<TRepositoryModel>(t);
    }
}
