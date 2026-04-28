using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    public static event Action<BattleLogEntry> LogAdded;

    [SerializeField] private float _delayBetweenLogs = 1f;
    private float _timer = 0f;

    private Queue<BattleLogEntry> _battleLogQueue = new();
    private List<BattleLogEntry> _battleLogs = new();

    public void Log(BattleLogEntry entry)
    {
        _battleLogs.Add(entry);
        _battleLogQueue.Enqueue(entry);
        Debug.Log(entry.Message);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _delayBetweenLogs && _battleLogQueue.Count > 0)
        {
            _timer = 0f;
            LogAdded?.Invoke(_battleLogQueue.Dequeue());
        }
            
    }
}
