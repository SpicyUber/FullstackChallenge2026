using Shared.Enumerators;
using System.Text.Json.Serialization;

namespace Shared.DataTransferObjects
{
    public class EffectDto
    {
        public long Id { get; private set; }
        public int Amount { get; private set; }

        public bool IsDebuff { get; private set; }
        public int Duration { get; private set; }

        public EffectType Type { get; private set; }

        [JsonConstructor]
        public EffectDto(
            long id,
            int amount, 
            bool isDebuff, 
            int duration, 
            EffectType type)
        {
            Id = id;
            Amount = amount;
            IsDebuff = isDebuff;
            Duration = duration;
            Type = type;
        }
    }
}