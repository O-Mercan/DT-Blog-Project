using dogus_study_case.Data;    
using dogus_study_case.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dogus_study_case.Repositories
{

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        protected ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}