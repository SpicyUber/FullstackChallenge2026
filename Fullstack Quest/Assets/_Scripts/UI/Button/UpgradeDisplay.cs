using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDisplay : MonoBehaviour, IClickHandler
{
    [SerializeField]
    private TextMeshProUGUI _tmp;
    [SerializeField]
    private string _statName;

    private Func<string> _onUpgradeGetStatLevel;
    private Action _upgrade;

    public int Priority => 0;
    public event Action WasUpgraded;

    private void OnEnable()
    {
        WasUpgraded += OnUpgrade;
    }

    private void OnDisable()
    {
        WasUpgraded -= OnUpgrade;
    }

    public void SetFunctionality(Func<string> onUpgradeGetStatLevel, Action upgrade)
    {
        _onUpgradeGetStatLevel = onUpgradeGetStatLevel;
        _upgrade = upgrade;

        OnUpgrade();
    }

    public void Handle()
    {
        _upgrade();
        WasUpgraded?.Invoke();
        
    }

    public void OnUpgrade()
    {
        _tmp.SetText(_statName + ": " + _onUpgradeGetStatLevel());
    }

}
