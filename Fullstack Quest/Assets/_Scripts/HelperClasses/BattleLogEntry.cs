using UnityEngine;

public class BattleLogEntry : MonoBehaviour
{
    public string Message { get; private set; }

    public BattleLogEntry(string characterName, string moveName)
    {
        Message = $"{characterName} used {moveName}";
    }
}
