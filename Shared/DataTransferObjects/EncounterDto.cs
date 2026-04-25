using Shared.Enumerators;
using System.Text.Json.Serialization;

namespace Shared.DataTransferObjects
{
    public class EncounterDto
    {
        public long Id { get; set; }
        
        public int Gold { get; set; }
        public int Xp { get; set; }

        public Difficulty Difficulty { get; set; }
        public CharacterDto Enemy { get; set; }

        public ItemDto? Item { get; set; }
        public EffectDto? Effect { get; set; }

        [JsonConstructor]
        public EncounterDto(
            long id,
            int gold,
            int xp,
            Difficulty difficulty, 
            CharacterDto enemy, 
            ItemDto? item,
            EffectDto? effect)
        {
            Id = id;
            Gold = gold;
            Xp = xp;
            Difficulty = difficulty;
            Enemy = enemy;
            Item = item;
            Effect = effect;
        }
    }
}
