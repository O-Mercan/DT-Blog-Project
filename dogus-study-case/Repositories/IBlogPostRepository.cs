using dogus_study_case.Models; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dogus_study_case.Repositories
{

    public interface IBlogPostRepository : IRepository<BlogPost>
    {

        Task<IEnumerable<BlogPost>> GetAllWithDetailsAsync();
        Task<BlogPost?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<BlogPost>> GetByCategoryIdAsync(int categoryId);

        Task DeleteAsync(BlogPost entity); 

        Task<int> SaveChangesAsync(); 
 
    }
}