using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EdriveAuto.Common;
using EdriveAuto.GenericPagination;

namespace EdriveAuto.GenericRepository;

public interface IGenericRepository<T> where T : class, IBaseRepositoryModel, new()
{
    DbSet<T> Entity { get; }

    List<T> GetAll(int pageNumber, int pageSize, string orderByString, out int count,
     Expression<Func<T, bool>>? predicate = null, string[]? includeTables = null);

    Task<List<T>> GetAllAsync(PaginatedList<T> pagination,
        Expression<Func<T, bool>>? predicate = null, string[]? includeTables = null);

    Task<Tuple<List<T>, int>> GetAllAsync(int pageNumber, int pageSize, string orderByString,
        Expression<Func<T, bool>>? predicate = null, string[]? includeTables = null);

    /// <summary>
    /// Finds an entity by ID
    /// </summary>
    /// <param name="id">ID of an entity to be found</param>
    /// <returns>Returns an entity if found, otherwise null</returns>
    Task<T?> GetAsync(int id);

    Task<T?> GetSingleByPredicateAsync(Expression<Func<T, bool>>? predicate);

    /// <summary>
    /// Only inserts a new entity into the database by checking if an entity already exists by the provided predicate.
    /// </summary>
    /// <param name="item">An Entity to be inserted.</param>
    /// <param name="findSinglePredicate">Predicate to check existing entity. Predicate must return a single entity to check against the existing item.</param>
    Task<CrudResponse> SaveAsync(T item, Expression<Func<T, bool>> findSinglePredicate);

    /// <summary>
    /// Only inserts a new entity into the database without validation.
    /// </summary>
    /// <param name="item">An Entity to be inserted.</param>
    Task<CrudResponse> SaveAsync(T item);

    /// <summary>
    /// Saves a list of objects
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<CrudResponse> SaveAllAsync(List<T> list);

    /// <summary>
    /// Only updates the passed element checking by ID. If ID is 0 returned an Error.
    /// </summary>
    /// <param name="item">An Entity to be updated.</param>
    Task<CrudResponse> UpdateAsync(T item);

    /// <summary>
    /// Inserts or Updates an element based on whether ID is provided or not.
    /// </summary>
    /// <param name="item">An Entity to be inserted or updated.</param>
    Task<CrudResponse> SaveOrUpdateAsync(T item);

    /// <summary>
    /// Deletes an entity based on ID.
    /// </summary>
    /// <param name="id">ID of an entity to be deleted.</param>
    /// <returns>Returns success message if an entity is found and deleted.</returns>
    Task<CrudResponse> DeleteAsync(int id);

    /// <summary>
    /// Counts elements in an entity. Count can be narrowed down by a predicate.
    /// </summary>
    /// <param name="predicate">Predicate to narrow down entity elements to be counted</param>
    /// <returns>Returns number of elements in an entity.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
}