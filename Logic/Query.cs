using Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Query<T, Q> : Operation<T, Q> where Q : class
    {
        public Query(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<T> Execute(Q? parameters = null)
        {
            var result = await ExecuteQuery(parameters);
            return result;
        }

        protected abstract Task<T> ExecuteQuery(Q? parameters);
    }
}
