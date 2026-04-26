using Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Command<T, Q> : Operation<T, Q> where Q :class
    {
        public Command(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<T> Execute(Q? parameters)
        {
            var result = await ExecuteCommand(_uow, parameters);
            await _uow.Save();
            return result;
        }

        protected abstract Task<T> ExecuteCommand(IUnitOfWork _uow, Q? parameters);
    }
}
