using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly FullstackQuestDbContext _dbContext;

        public GenericRepository(FullstackQuestDbContext dbContext) { _dbContext = dbContext; }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>();
        }
    }
}
