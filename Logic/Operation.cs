using Infrastructure.Persistence.UnitOfWork;

namespace Logic
{
    public abstract class Operation<T, Q> where Q : class
    {
        protected IUnitOfWork _uow;
        public Operation(IUnitOfWork uow) { _uow = uow; }
        public abstract Task<T> Execute(Q? parameters = null);
    }
}
