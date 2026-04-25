using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CharacterMove
    {
        public long CharacterId { get; set; }
        public long MoveId { get; set; }

        [Required]
        public Character Character { get; set; }

        [Required]
        public Move Move { get; set; }
    }
}