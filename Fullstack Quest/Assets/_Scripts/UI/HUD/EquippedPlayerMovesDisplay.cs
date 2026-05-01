using Shared.DataTransferObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippedPlayerMovesDisplay : MonoBehaviour, IHUDElement
{
    [SerializeField] private MoveDetailsDisplay _detailsDisplay;
    [SerializeField] private Image _panelImage;
    [SerializeField] private TextMeshProUGUI[] _textMeshes;

    private BaseCharacter _hero;
    private MoveDto[] _moves;
    
    private Color _startColor;

    private void OnEnable()
    {
        BattleEvents.PlayerPickedMove += OnPlayerPickedMove;
        BattleEvents.TurnStarted += Show;
    }

    private void OnDisable()
    {
        BattleEvents.PlayerPickedMove -= OnPlayerPickedMove;
        BattleEvents.TurnStarted -= Show;
    }

    private void Start()
    {
        _startColor = _panelImage.color;
    }

    private void Hide()
    {
        _panelImage.color = new Color(0, 0, 0, 0);

        foreach(Transform child in transform)
            child.gameObject.SetActive(false);

        _detailsDisplay.Clear();
    }

    private void Show()
    {
        if(!_hero.IsMyTurn) return;

        _panelImage.color = _startColor;

        foreach(Transform child in transform)
            child.gameObject.SetActive(true);
    }

    private void OnPlayerPickedMove(long _)
    {
        Hide();
    }

    public void Initialize(BaseCharacter hero, BaseCharacter monster)
    {
        _hero = hero;
        _moves = hero.Moves;

        for(int i = 0; i < _textMeshes.Length; i++)
        {
            _textMeshes[i].SetText(_moves[i].Name);
        }

        Show();
    }

    public void Hover(int slot)
    {
        _detailsDisplay.Display(_moves[slot]);
    }

    public void Selected(int slot)
    {
        BattleEvents.InvokePlayerPickedMove(_moves[slot].Id);
    }

    public void Refresh()
    {

    }

}
