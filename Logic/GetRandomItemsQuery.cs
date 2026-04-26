using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class GetRandomItemsQuery : Query<List<ItemDto>, object>
    {
        public GetRandomItemsQuery(IUnitOfWork uow) : base(uow)
        {
        }

        protected override async Task<List<ItemDto>> ExecuteQuery(object? parameters)
        {
            return await _uow.ItemRepository.Query()
           .OrderBy(i => Guid.NewGuid())
           .Take(4)
           .Select(i => new ItemDto(i.Id, i.Name, i.Price, i.Type, i.AttackDelta, i.DefenseDelta, i.HealthDelta, i.ManaDelta, i.MagicDelta))
           .ToListAsync();
        }
    }
}
