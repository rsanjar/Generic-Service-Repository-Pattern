using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EdriveAuto.Common;
using EdriveAuto.Common.Enums;
using EdriveAuto.GenericPagination;

namespace EdriveAuto.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseRepositoryModel, new()
{
    #region ctor

    private readonly DbContext _context;
    private const string ID = nameof(IBaseRepositoryModel.ID);

    public GenericRepository(DbContext context)
    {
        _context = context;
    }

    #endregion

    public DbSet<TEntity> Entity => _context.Set<TEntity>();

    public virtual List<TEntity> GetAll(int pageNumber, int pageSize, string orderByString,
        out int count, Expression<Func<TEntity, bool>>? predicate = null, string[]? includeTables = null)
    {
        var query = predicate != null ? Entity.Where(predicate) : Entity;

        query.IncludeTables(includeTables);

        orderByString = SetOrderBy(orderByString);

        var result = query.Paginate(pageNumber, pageSize, orderByString, out count);

        return result;
    }
    
    public virtual async Task<List<TEntity>> GetAllAsync(PaginatedList<TEntity> pagination,
        Expression<Func<TEntity, bool>>? predicate = null, string[]? includeTables = null)
    {
        var query = predicate != null ? Entity.Where(predicate) : Entity;

        query.IncludeTables(includeTables);
        
        pagination.OrderBy = SetOrderBy(pagination.OrderBy);

        var result = await query.PaginateAsync(pagination);

        return result;
    }

    public virtual async Task<Tuple<List<TEntity>, int>> GetAllAsync(int pageNumber, int pageSize,
        string orderByString, Expression<Func<TEntity, bool>>? predicate = null, string[]? includeTables = null)
    {
        var query = predicate != null ? Entity.Where(predicate) : Entity;

        query.IncludeTables(includeTables);

        orderByString = SetOrderBy(orderByString);

        var result = await query.PaginateAsync(pageNumber, pageSize, orderByString);

        return result;
    }

    public virtual async Task<TEntity?> GetAsync(int id)
    {
        return await Entity.FirstOrDefaultAsync(c => c.ID == id);
    }

    public virtual async Task<TEntity?> GetSingleByPredicateAsync(Expression<Func<TEntity, bool>>? predicate)
    {
        TEntity? result = null;

        if (predicate != null)
        {
            result = await Entity.FirstOrDefaultAsync(predicate);
        }

        return result;
    }

    /// <summary>
    /// Save a new instance only if it doesn't exist
    /// </summary>
    /// <param name="item"></param>
    /// <param name="findSinglePredicate"></param>
    /// <returns></returns>
    public virtual async Task<CrudResponse> SaveAsync(TEntity item, Expression<Func<TEntity, bool>> findSinglePredicate)
    {
        var t = Entity;

        var query = await t.SingleOrDefaultAsync(findSinglePredicate);

        if (query == null)
        {
            item.InitDateLoggable();

            t.Attach(item);
            await t.AddAsync(item);

            await _context.SaveChangesAsync();

            return Result(Crud.Success);
        }

        item.ID = query.ID;

        return Result(Crud.DuplicateEntryError);
    }
    
    public virtual async Task<CrudResponse> SaveAsync(TEntity item)
    {
        var t = Entity;

        item.InitDateLoggable();

        t.Attach(item);
        await t.AddAsync(item);
        await _context.SaveChangesAsync();

        return Result(Crud.Success);
    }

    public virtual async Task<CrudResponse> SaveAllAsync(List<TEntity> list)
    {
        var t = Entity;

        list.ForEach(c => c.InitDateLoggable());

        t.AttachRange(list);
        await t.AddRangeAsync(list);
        await _context.SaveChangesAsync();

        return Result(Crud.Success);
    }

    public virtual async Task<CrudResponse> UpdateAsync(TEntity item)
    {
        if (item.ID <= 0)
            return Result(Crud.Error);

        var t = Entity;

        var query = await t.SingleOrDefaultAsync(c => c.ID == item.ID);

        if (query != null)
        {
	        if (item is IDateUpdatedLoggable dateUpdated)
                dateUpdated.DateUpdated = DateTime.UtcNow;
            
            _context.Entry(query).CurrentValues.SetValues(item);

            if (item is IDateCreatedLoggable)
	            _context.Entry(query).Property(c => ((IDateCreatedLoggable)c).DateCreated).IsModified = false;

            await _context.SaveChangesAsync();

            item.ID = query.ID;

            return Result(Crud.Success);
        }

        return Result(Crud.ItemNotFoundError);
    }

    public virtual async Task<CrudResponse> SaveOrUpdateAsync(TEntity item)
    {
        return item.ID <= 0 ? 
	        await SaveAsync(item) : 
	        await UpdateAsync(item);
    }

    public virtual async Task<CrudResponse> DeleteAsync(int id)
    {
        var t = Entity;
        var query = await t.FirstOrDefaultAsync(c => c.ID == id);

        if (query != null)
        {
            t.Remove(query);
            await _context.SaveChangesAsync();

            return Result(Crud.Success);
        }

        return Result(Crud.ItemNotFoundError);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
	    int count = predicate == null ? 
		    await Entity.CountAsync() : 
		    await Entity.CountAsync(predicate);
        
        return count;
    }

    private static CrudResponse Result(Crud crud)
    {
        return new CrudResponse(crud);
    }
    
    private static string SetOrderBy(string? orderBy)
    {
	    if (string.IsNullOrWhiteSpace(orderBy))
		    orderBy = ID;

	    return orderBy;
    }
}