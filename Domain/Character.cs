using Shared.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Character
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Health { get; set; }

        [Range(0, int.MaxValue)]
        public int Mana { get; set; }

        [Range(0, int.MaxValue)]
        public int Attack { get; set; }

        [Range(0, int.MaxValue)]
        public int Defense { get; set; }

        [Range(0, int.MaxValue)]
        public int Magic { get; set; }

        public CharacterType Type { get; set; }
        public bool IsHero { get; set; }

        public ICollection<CharacterMove> Moves { get; set; } = [];
    }
}
