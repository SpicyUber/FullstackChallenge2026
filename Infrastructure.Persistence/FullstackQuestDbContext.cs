using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastructure.Persistence
{
    public class FullstackQuestDbContext : DbContext
    {
        public FullstackQuestDbContext(DbContextOptions<FullstackQuestDbContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<CharacterMove> CharacterMoves { get; set; }
        public DbSet<Effect> Effects { get; set; }
        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<Item> Item { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CreateRelations(builder);
            ConvertEnumsIntoStrings(builder);


        }

        private void CreateRelations(ModelBuilder builder)
        {
            builder.Entity<Character>().HasKey(c => c.Id);

            builder.Entity<CharacterMove>().HasKey(cm => new { cm.MoveId, cm.CharacterId });
            builder.Entity<CharacterMove>().HasOne(cm => cm.Character).WithMany(c => c.Moves);

            builder.Entity<CharacterMove>().HasOne(cm => cm.Move).WithMany();

            builder.Entity<Move>().HasKey(m => m.Id);
            builder.Entity<Move>().HasOne(m => m.Effect).WithMany().IsRequired(false);

            builder.Entity<Encounter>().HasKey(e => e.Id);
            builder.Entity<Encounter>().HasOne(e => e.Enemy).WithMany();
            builder.Entity<Encounter>().HasOne(e => e.Effect).WithMany().IsRequired(false);
            builder.Entity<Encounter>().HasOne(e => e.Item).WithMany().IsRequired(false); ;

            builder.Entity<Item>().HasKey(i => i.Id);

            builder.Entity<Effect>().HasKey(e => e.Id);
        }

        private void ConvertEnumsIntoStrings(ModelBuilder builder)
        {
            builder.Entity<Character>().Property(c => c.Type).HasConversion<string>();

            builder.Entity<Move>().Property(m => m.DamageType).HasConversion<string>();
            builder.Entity<Move>().Property(m => m.VFXType).HasConversion<string>();
            builder.Entity<Move>().Property(m => m.SFXType).HasConversion<string>();
            builder.Entity<Move>().Property(m => m.Element).HasConversion<string>();

            builder.Entity<Encounter>().Property(e => e.Difficulty).HasConversion<string>();

            builder.Entity<Item>().Property(i => i.Type).HasConversion<string>();

            builder.Entity<Effect>().Property(e => e.Type).HasConversion<string>();
        }
    }
}
