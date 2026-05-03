using Shared.DataTransferObjects;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour, IClickHandler
{
    [SerializeField]
    private TextMeshProUGUI _name, _attack, _defense, _magic, _health, _mana, _price;
    [SerializeField]
    private AudioClip _buySound, _brokeSound;
    [SerializeField]
    private Image _panel;

    public event Action ItemBought;
    private ItemDto _item;


    public int Priority => 0;

    public void Handle()
    {
        if(GameManager.Instance.TryBuy(_item))
        {
            AudioSource.PlayClipAtPoint(_buySound, Camera.main.transform.position);
            ItemBought?.Invoke();
            _item = null;
            Refresh();
        }
        else
        {
            AudioSource.PlayClipAtPoint(_brokeSound, Camera.main.transform.position);
        }

    }

    public void SetItem(ItemDto item)
    {
        _item = item;
        Refresh();
    }

    private void Refresh()
    {
        if(_item == null)
        {
            SetToSoldOut();
            return;
        }

        _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, 0);

        _name.SetText(_item.Name + "");
        _attack.SetText(_item.AttackDelta + "");

        _defense.SetText(_item.DefenseDelta + "");
        _magic.SetText(_item.MagicDelta + "");

        _health.SetText(_item.HealthDelta + "");
        _mana.SetText(_item.ManaDelta + "");

        _price.SetText("BUY (" + _item.Price + ")");

        if(IsPurchased)
            SetToSoldOut();
    }

    private void SetToSoldOut()
    {
        _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, 1);
        _price.SetText("SOLD OUT");
        _item = null;
    }

    private bool IsPurchased => GameManager.Instance.HeroOwnedItems.Any(item => item.Id == _item.Id);
}
