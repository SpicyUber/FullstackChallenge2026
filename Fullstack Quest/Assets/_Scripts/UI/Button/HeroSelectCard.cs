using Shared.DataTransferObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroSelectCard : MonoBehaviour, IClickHandler
{
    [SerializeField]
    private TextMeshProUGUI _heroName, _heroDescription, _moveNames;

    [SerializeField]
    private Image _heroIcon;

    private CharacterDto _heroCharacter;

    private float _wiggleSize = 5;
    private float _randomOffset;

    private RectTransform _rectTransform;
    private float _originalY;

    public int Priority => 99;

    public void Handle()
    {
        GameManager.Instance.ChooseHeroAndStartGame(_heroCharacter);
        UIManager.Instance.CloseMenu(typeof(CharacterSelectMenuData));
    }

    public void SetCharacter(CharacterDto heroCharacter)
    {
        _heroCharacter = heroCharacter;
        _heroIcon.sprite = LookupDictionaries.Instance.GetCharacterSprite(heroCharacter.Type);

        _heroDescription.SetText(
        $"ATT: {heroCharacter.Attack} "
        + $"DEF: {heroCharacter.Defense} "
        + $"MAG: {heroCharacter.Magic} "
        + $"HP: {heroCharacter.Health} "
        + $"MP: {heroCharacter.Mana}");

        string names = "MOVES:\n";

        foreach(var move in _heroCharacter.Moves)
            names += move.Name.ToUpper()+"\n";

        _moveNames.SetText(names);
    
    }

    private void Start()
    {
        _randomOffset = Random.value;

        _rectTransform = GetComponent<RectTransform>();
        _originalY = _rectTransform.anchoredPosition.y;
    }

    private void Update() => CardWiggle();

    private void CardWiggle()
        => _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _originalY + _wiggleSize * Mathf.Sin(Time.time + _randomOffset));
}
