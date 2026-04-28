using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public class BattleLogDisplay : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void OnEnable()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        BattleLog.LogAdded += ShowLog;
    }

    private void ShowLog(BattleLogEntry entry) 
    {
        _tmp.SetText(entry.Message);
    }

    private void OnDisable()
    {
        BattleLog.LogAdded -= ShowLog;
    }
}
