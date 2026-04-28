using Shared.DataTransferObjects;
using TMPro;
using UnityEngine;

public class EquippedPlayerMovesDisplay : MonoBehaviour, IHUDElement
{
    [SerializeField] MoveDetailsDisplay detailsDisplay;

    private BaseCharacter _hero;
    private MoveDto[] _moves;
    private TextMeshProUGUI[] _tmps;


    public void Initialize(BaseCharacter hero, BaseCharacter monster)
    {
        _hero = hero;
        _moves = hero.Moves;
        _tmps = GetComponentsInChildren<TextMeshProUGUI>();

        for(int i = 0; i < _tmps.Length; i++)
        {
            _tmps[i].SetText(_moves[i].Name);
        }
    }

    public void Hover(int slot)
    {
        detailsDisplay.Display(_moves[slot]);
    }

    public void Selected(int slot)
    {
        BattleEvents.InvokePlayerPickedMove(_moves[slot].Id);
    }

    public void Refresh()
    {

    }
}
