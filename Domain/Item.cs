using Shared.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        public ItemType Type { get; set; }

        public int AttackDelta {  get; set; }

        public int DefenseDelta { get; set; }

        public int HealthDelta { get; set; }

        public int ManaDelta { get; set; }

        public int MagicDelta { get; set; }
    }
}
