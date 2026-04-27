using Newtonsoft.Json;

namespace Shared.DataTransferObjects
{
    public class CharacterBattleStateDto
    {
        public long Id { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentMana { get; set; }
        public int MaxHealth { get ; set; }
        public int MaxMana { get; set; }

        public CharacterBattleStateDto() { }

        [JsonConstructor]
        public CharacterBattleStateDto(long id, int currentHealth, int currentMana, int maxHealth, int maxMana)
        {
            Id = id;
            CurrentHealth = currentHealth;
            CurrentMana = currentMana;
            MaxHealth = maxHealth;
            MaxMana = maxMana;

        }
    }
}
