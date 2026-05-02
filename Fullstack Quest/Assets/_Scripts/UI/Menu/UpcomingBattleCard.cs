using DG.Tweening;
using Shared.DataTransferObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpcomingBattleCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI _enemyName, _enemyDescription, _effectName, _itemName, _xpReward, _goldReward;
    [SerializeField]
    private Image _enemyIcon, _effectIcon;

    private bool _goesToRightNode;

    private float _wiggleSize = 5;
    private float _randomOffset;

    private RectTransform _rectTransform;
    private float _originalY;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GoToTextBattle(_goesToRightNode);
        UIManager.Instance.CloseMenu(typeof(OverviewMenuData));
    }

    public void SetEncounter(EncounterDto encounter, bool isRightChoice)
    {
        _goesToRightNode = isRightChoice;
        int loopNumber = GameManager.Instance.LoopStatMultiplier;

        _enemyName.SetText(encounter.Enemy.Name.ToUpper());
        _enemyDescription.SetText(
            $"ATT: {encounter.Enemy.Attack * loopNumber} "
            + $"DEF: {encounter.Enemy.Defense * loopNumber} "
            + $"MAG: {encounter.Enemy.Magic * loopNumber} "
            + $"HP: {encounter.Enemy.Health * loopNumber} "
            + $"MP: {encounter.Enemy.Mana * loopNumber}"
            );

        _enemyIcon.sprite = LookupDictionaries.Instance.GetCharacterSprite(encounter.Enemy.Type);

        if(encounter.Effect != null)
        {
            _effectIcon.sprite = LookupDictionaries.Instance.GetBuffSprite(encounter.Effect.Type, encounter.Effect.IsDebuff);
            _effectName.SetText("EFFECT: " + encounter.Effect.Type.ToString().Replace("_", " "));
            _effectIcon.color = new(1, 1, 1, 1);
        }
        else
        {
            _effectName.SetText("EFFECT: NONE");
            _effectIcon.color = new(1, 1, 1, 0);
        }

        if(encounter.Item != null)
        {
            _itemName.SetText("ITEM: " + encounter.Item.Name.ToUpper());
        }
        else
        {
            _itemName.SetText("ITEM: NONE");
        }

        _xpReward.SetText(encounter.Xp.ToString());
        _goldReward.SetText(encounter.Gold.ToString());
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
