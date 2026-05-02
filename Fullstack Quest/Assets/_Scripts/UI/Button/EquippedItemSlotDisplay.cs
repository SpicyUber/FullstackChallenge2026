using Shared.DataTransferObjects;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedItemSlotDisplay : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private TextMeshProUGUI _tmp;
    [SerializeField]
    private AudioClip _dropSound;

    private Hero _hero;
    private int _slotIndex;

    public event Action<long> ItemWasEquipped;
    public ItemDto Item { get; private set; }

    public void OnDrop(PointerEventData eventData)
    {
        DropAction(eventData);
    }

    public void Initialize(Hero hero, ItemDto item, int slotIndex)
    {
        Item = item;

        if(Item != null)
            _tmp.SetText(Item.Name);
        else
            _tmp.SetText("Empty Slot");

        _hero = hero;
        _slotIndex = slotIndex;
    }

    protected virtual void DropAction(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null) return;
        var item = eventData.pointerDrag.GetComponent<UnequippedItemDisplay>().Item;

        Initialize(_hero, item, _slotIndex);
        _hero.Items[_slotIndex] = item;
        AudioSource.PlayClipAtPoint(_dropSound, Camera.main.transform.position);

        ItemWasEquipped.Invoke(item.Id);

    }
}
