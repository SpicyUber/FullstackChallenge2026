using Newtonsoft.Json;

namespace Shared.DataTransferObjects
{
    public class EncounterTreeNodeDto
    {
        public EncounterDto Encounter { get; private set; }

        public EncounterTreeNodeDto? Left { get; private set; }

        public EncounterTreeNodeDto? Right { get; private set; }

        [JsonConstructor]
        public EncounterTreeNodeDto(EncounterDto encounter, EncounterTreeNodeDto? left, EncounterTreeNodeDto? right)
        {
            Encounter = encounter;
            Left = left;
            Right = right;
        }
    }
}
