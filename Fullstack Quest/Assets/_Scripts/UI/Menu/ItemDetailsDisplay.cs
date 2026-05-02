using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using TMPro;
using UnityEngine;

public class ItemDetailsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;
    private readonly string _descreaseColor = "red";
    private readonly string _increaseColor = "blue";
    private readonly string _noChangeColor = "#808080";

    public void Display(ItemDto item, BaseCharacter hero)
    {
        string itemInfo = "";


        string color;

        color = CalculateColorFromDelta(item.AttackDelta);
        itemInfo += $"Attack: {hero.Attack} <color={color}>{CalculateSignFromDelta(item.AttackDelta)}{Mathf.Abs(item.AttackDelta)}</color>\n";

        color = CalculateColorFromDelta(item.DefenseDelta);
        itemInfo += $"Defense: {hero.Defense} <color={color}>{CalculateSignFromDelta(item.DefenseDelta)}{Mathf.Abs(item.DefenseDelta)}</color>\n";

        color = CalculateColorFromDelta(item.MagicDelta);
        itemInfo += $"Magic: {hero.Magic} <color={color}>{CalculateSignFromDelta(item.MagicDelta)}{Mathf.Abs(item.MagicDelta)}</color>\n";

        color = CalculateColorFromDelta(item.HealthDelta);
        itemInfo += $"Health: {hero.Health} <color={color}>{CalculateSignFromDelta(item.HealthDelta)}{Mathf.Abs(item.HealthDelta)}</color>\n";

        color = CalculateColorFromDelta(item.ManaDelta);
        itemInfo += $"Mana: {hero.Mana} <color={color}>{CalculateSignFromDelta(item.ManaDelta)}{Mathf.Abs(item.ManaDelta)}</color>\n";

        _tmp.SetText(itemInfo);
    }

    private object CalculateSignFromDelta(int delta) => delta < 0 ? "-" : "+";

    public void Clear() => _tmp.SetText("");

    private string CalculateColorFromDelta(int delta)
    {
        if(delta > 0) return _increaseColor;
        if(delta < 0) return _descreaseColor;
        return _noChangeColor;
    }
}
