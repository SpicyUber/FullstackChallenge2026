using UnityEngine;

public class EquippedItemSlotsLoader : MonoBehaviour
{
    [SerializeField]
    private EquippedItemSlotDisplay[] _slotDisplays;
    private ItemDetailsDisplay _itemDetailsDisplay;
    private Hero _hero;

    public void Load(Hero hero, ItemDetailsDisplay itemDetailsDisplay)
    {
        _hero = hero;
        _itemDetailsDisplay = itemDetailsDisplay;

        for(int i = 0; i < Hero.HeroItemSlots; i++)
        {
            _slotDisplays[i].Initialize(hero, hero.Items[i], i);
            _slotDisplays[i].ItemWasEquipped += EnforceItemUnique;
            _slotDisplays[i].ItemWasEquipped += ClearItemDetails;
        }

    }

    public void ClearItems()
    {
        for(int i = 0; i < Hero.HeroItemSlots; i++)
        {
            _hero.Items[i] = null;
            _slotDisplays[i].Initialize(_hero, null, i);
        }

        ClearItemDetails(0);
    }

    private void ClearItemDetails(long _)
    {
        _itemDetailsDisplay.Clear();
    }

    private void EnforceItemUnique(long itemId)
    {
        int foundIndex = -1;

        for(int i = 0; i < Hero.HeroItemSlots; i++)
        {
            if(_hero.Items[i] != null && _hero.Items[i].Id == itemId)
            {
                if(foundIndex<0)
                {
                    foundIndex = i;
                }
                else
                {
                    _hero.Items[foundIndex] = null;
                    _slotDisplays[foundIndex].Initialize(_hero, null, foundIndex);
                    return;
                }

            }

        }
    }

    public void OnDestroy()
    {
        for(int i = 0; i < Hero.HeroItemSlots; i++)
        {
            _slotDisplays[i].ItemWasEquipped -= EnforceItemUnique;
            _slotDisplays[i].ItemWasEquipped += ClearItemDetails;
        }

    }
}
