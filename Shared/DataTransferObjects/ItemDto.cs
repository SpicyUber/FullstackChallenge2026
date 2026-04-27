using Shared.Enumerators;
using Newtonsoft.Json;

namespace Shared.DataTransferObjects
{
    public class ItemDto
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public int Price { get; private set; }

        public ItemType Type { get; private set; }
        public int AttackDelta { get; private set; }

        public int DefenseDelta { get; private set; }
        public int HealthDelta { get; private set; }

        public int ManaDelta { get; private set; }
        public int MagicDelta { get; private set; }

        [JsonConstructor]
        public ItemDto(
            long id, 
            string name, 
            int price, 
            ItemType type, 
            int attackDelta, 
            int defenseDelta, 
            int healthDelta, 
            int manaDelta, 
            int magicDelta)
        {
            Id = id;
            Name = name;
            Price = price;
            Type = type;
            AttackDelta = attackDelta;
            DefenseDelta = defenseDelta;
            HealthDelta = healthDelta;
            ManaDelta = manaDelta;
            MagicDelta = magicDelta;
        }
    }
}
