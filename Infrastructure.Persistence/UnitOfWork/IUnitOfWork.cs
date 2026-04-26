using Domain;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<CharacterMove> CharacterMoveRepository { get; }
        public IRepository<Encounter> EncounterRepository { get; }
        public IRepository<Item> ItemRepository { get; }
        public IRepository<Character> CharacterRepository { get;  }

        Task<int> Save();
    }
}
