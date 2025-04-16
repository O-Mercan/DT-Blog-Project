using dogus_study_case.Data;      
using dogus_study_case.Models;   
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace dogus_study_case.Repositories
{
    public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
    {
        protected ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public BlogPostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BlogPost>> GetAllWithDetailsAsync()
        {
            return await AppContext.BlogPosts
                                 .Include(p => p.Category)
                                 .Include(p => p.User)
                                 .OrderByDescending(p => p.PublicationDate)
                                 .ToListAsync();
        }

        public async Task<BlogPost?> GetByIdWithDetailsAsync(int id)
        {
            return await AppContext.BlogPosts
                                 .Include(p => p.Category)
                                 .Include(p => p.User)
                                 .Include(p => p.Comments)
                                 .ThenInclude(c => c.User)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<BlogPost>> GetByCategoryIdAsync(int categoryId)
        {
            return await AppContext.BlogPosts
                                 .Include(p => p.User) 
                                 .Where(p => p.CategoryId == categoryId)
                                 .OrderByDescending(p => p.PublicationDate)
                                 .ToListAsync();
        }
        public Task DeleteAsync(BlogPost entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            AppContext.BlogPosts.Remove(entity); 
            return Task.CompletedTask; 
        }
        public async Task<int> SaveChangesAsync()
        {
            return await AppContext.SaveChangesAsync(); 
        }
    }
}