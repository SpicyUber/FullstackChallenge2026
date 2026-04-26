using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> Query();
    }
}
