
namespace ReceiptVoucher.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<T> FindAsync(Expression<Func<T, bool>> match, string[] includes = null);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, int take, int skip);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? PageSize, int? PageNumber, string? search,
           Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null);

        /// <summary>
        /// <b>WALEED MOHAMMED</b>  , Edited At 2024-02-26   12:49 PM 
        /// <br></br>
        /// This Function Asynchronously finds all entities of type T that satisfy the specified criteria and maps them to the specified destination type.
        /// </summary>
        /// <typeparam name="TDestination">The type of the destination objects. This is typically a DTO (Data Transfer Object).</typeparam>
        /// <param name="criteria">A lambda expression representing the criteria that the entities should satisfy.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of the destination type.</returns>
        /// <remarks>
        /// This method uses the ProjectTo method from AutoMapper to map from the entity type to the destination type.
        /// The mapping configuration should be defined in an AutoMapper profile.
        /// </remarks>
        /// <example>
        /// Here's how you can use this method:
        /// <code>
        /// IEnumerable&lt;OperationWithRelatedData_DTO&gt; operations = await _unitOfWork.Operations.FindAllAsync&lt;OperationWithRelatedData_DTO&gt;(
        ///     op => op.Deleted == false
        /// );
        /// </code>
        /// This will return a list of OperationWithRelatedData_DTO objects, with each Operation entity mapped to an OperationWithRelatedData_DTO object according to the mapping defined in your AutoMapper profile.
        /// </example>
        Task<IEnumerable<TDestination>> FindAllAsync<TDestination>(Expression<Func<T, bool>> criteria);

        Task<T> AddOneAsync(T entity);

        IEnumerable<T> AddRange(IEnumerable<T> entities);

        T? Update(T entity);

        Task<bool> DeleteAsync(int id);
        void DeleteRange(IEnumerable<T> entities);

        void Attach(T entity);

        int Count();
        int Count(Expression<Func<T, bool>> match);

        Task<bool> AnyAsync(Expression<Func<T, bool>> match);


        
    }
}
