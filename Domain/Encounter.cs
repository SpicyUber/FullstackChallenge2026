using Shared.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Encounter
    {
        public long Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Gold { get; set; }

        [Range(0, int.MaxValue)]
        public int Xp { get; set; }

        public Difficulty Difficulty { get; set; }

        [Required]
        public Character Enemy { get; set; }
        public long EnemyId {  get; set; }

        public Item? Item { get; set; }
        public long? ItemId { get; set; }

        public Effect? Effect { get; set; }
        public long? EffectId { get; set; }
    }
}
