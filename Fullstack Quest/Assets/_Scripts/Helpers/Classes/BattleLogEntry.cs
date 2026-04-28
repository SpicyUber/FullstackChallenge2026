using UnityEngine;

public class BattleLogEntry
{
    public string Message { get; private set; }

    public BattleLogEntry(string characterName, string moveName)
    {
        Message = $"{characterName} used {moveName}";
    }
}
