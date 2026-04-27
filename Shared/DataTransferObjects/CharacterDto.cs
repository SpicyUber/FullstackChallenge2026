using Shared.Enumerators;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Shared.DataTransferObjects
{
    public class CharacterDto
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public int Health { get; private set; }
        public int Mana { get; private set; }

        public int Attack { get; private set; }
        public int Defense { get; private set; }

        public int Magic { get; private set; }
        public CharacterType Type { get; private set; }

        public List<MoveDto> Moves { get; private set; } = new List<MoveDto>();

        [JsonConstructor]
        public CharacterDto(
            long id,
            string name,
            int health,
            int mana,
            int attack,
            int defense,
            int magic,
            CharacterType type,
            List<MoveDto> moves)
        {
            Id = id;
            Name = name;
            Health = health;
            Mana = mana;
            Attack = attack;
            Defense = defense;
            Magic = magic;
            Type = type;
            Moves = moves ?? new List<MoveDto>();
        }
    }
}
