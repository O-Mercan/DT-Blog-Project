using System.Linq.Expressions; 
using System.Threading.Tasks;
using System.Collections.Generic; 
using System; 

namespace dogus_study_case.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Get by ID (Asenkron)
        Task<T?> GetByIdAsync(int id); 

        // Get All (Asenkron)
        Task<IEnumerable<T>> GetAllAsync();

        // Find using an expression (Asenkron) 
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add a single entity (Asenkron)
        Task AddAsync(T entity);

        // Add multiple entities (Asenkron)
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update an entity 
        void Update(T entity);

        // Remove a single entity (Senkron)
        void Delete(T entity);

        // Remove multiple entities (Senkron)
        void DeleteRange(IEnumerable<T> entities);

        // Optional
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<int> SaveChangesAsync();
    }
}
