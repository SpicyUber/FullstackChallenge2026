using Shared.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Effect
    {
        public long Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Amount { get; set; }

        [Range(0, int.MaxValue)]
        public int Duration { get; set; }

        public bool IsDebuff {  get; set; }
        EffectType Type { get; set; }
    }
}