using Shared.Enumerators;
using Newtonsoft.Json;

namespace Shared.DataTransferObjects
{
    public class MoveDto
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public DamageType DamageType { get; private set; }
        public int DamageScaling {  get; private set; }

        public int SelfHealingScaling { get; private set; }

        public int HealthCost { get; private set; }
        public int ManaCost { get; private set; }

        public Element Element { get; private set; }
        public EffectDto Effect {  get; private set; }
        
        public bool IsVFXAndEffectSelfCast { get; private set; }
        public MoveVFXType VFXType { get; private set; }

        public MoveSFXType SFXType { get; private set; }

        [JsonConstructor]
        public MoveDto(
            long id,
            string name,
            DamageType damageType, 
            int damageScaling,
            int selfHealingScaling, 
            int healthCost, 
            int manaCost, 
            Element element, 
            EffectDto effect, 
            bool isVFXAndEffectSelfCast,
            MoveVFXType vfxType, 
            MoveSFXType sfxType)
        {
            Id = id;
            Name = name;
            DamageType = damageType;
            DamageScaling = damageScaling;
            SelfHealingScaling = selfHealingScaling;
            HealthCost = healthCost;
            ManaCost = manaCost;
            Element = element;
            Effect = effect;
            IsVFXAndEffectSelfCast = isVFXAndEffectSelfCast;
            VFXType = vfxType;
            SFXType = sfxType;
        }
    }
}
