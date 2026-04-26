using Domain;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FullstackQuestDbContext _dbContext;

        public UnitOfWork(FullstackQuestDbContext dbContext)
        {
            _dbContext = dbContext;

            CharacterMoveRepository = new GenericRepository<CharacterMove>(_dbContext);
            ItemRepository = new GenericRepository<Item>(_dbContext);
            EncounterRepository = new GenericRepository<Encounter>(_dbContext);
            CharacterRepository = new GenericRepository<Character>(_dbContext);

        }

        public IRepository<CharacterMove> CharacterMoveRepository { get; }
        public IRepository<Item> ItemRepository { get; }
        public IRepository<Encounter> EncounterRepository { get; }
        public IRepository<Character> CharacterRepository { get; }

        public async Task<int> Save()
            => await _dbContext.SaveChangesAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
