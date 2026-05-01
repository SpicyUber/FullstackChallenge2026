using System;
using UnityEngine;

public class BattleLogEntry
{
    public string Message { get; private set; }
    private string _characterName, _moveName;

    public BattleLogEntry(string characterName, string moveName)
    {
        _characterName = characterName;
        _moveName = moveName;

        SetDefault();
    }

    public void SetDefault() => Message = $"{_characterName} used {_moveName}";

    public void SetFailed() => Message = $"{_characterName} couldn't afford {_moveName}";
}
