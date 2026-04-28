using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private BaseCharacter _hero;
    [SerializeField] private BaseCharacter _monster;

    private IHUDElement[] _hudElements;

    private void Start()
    {
        _hudElements = GetComponentsInChildren<IHUDElement>();
        Hide();
    }

    private void OnEnable()
    {
        BattleEvents.BattleStarted += InitializeElements;
        BattleEvents.BattleStarted += Show;
        BattleEvents.MonsterDied += Hide;
        BattleEvents.PlayerDied += Hide;
    }

    private void Show()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void InitializeElements() 
    {
        foreach(var element in _hudElements)
            element.Initialize(_hero,_monster);
    }

    private void Refresh()
    {
        foreach(var element in _hudElements)
            element.Refresh();
    }

    private void Hide()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        BattleEvents.BattleStarted -= Show;
        BattleEvents.BattleStarted -= InitializeElements;
        BattleEvents.MonsterDied -= Hide;
        BattleEvents.PlayerDied -= Hide;
    }


}
