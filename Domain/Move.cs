using Shared.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Move
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DamageType DamageType { get; set; }
        public Element Element { get; set; }

        [Range(0, int.MaxValue)]
        public int Damage { get; set; }

        [Range(0, int.MaxValue)]
        public int SelfHealingAmount { get; set; }

        [Range(0, int.MaxValue)]
        public int HealthCost { get; set; }

        [Range(0, int.MaxValue)]
        public int ManaCost { get; set; }

        public long? EffectId { get; set; }
        public Effect? Effect { get; set; }

        public MoveVFXType VFXType { get; set; }
        public MoveSFXType SFXType { get; set; }

        public bool isVFXSelfCast { get; set; }
    }
}
