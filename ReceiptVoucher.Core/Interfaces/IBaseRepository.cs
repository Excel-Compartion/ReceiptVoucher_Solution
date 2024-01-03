
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

        Task<T> AddOneAsync(T entity);

        IEnumerable<T> AddRange(IEnumerable<T> entities);

        T Update(T entity);

        Task<bool> DeleteAsync(int id);
        void DeleteRange(IEnumerable<T> entities);

        void Attach(T entity);

        int Count();
        int Count(Expression<Func<T, bool>> match);

    }
}
