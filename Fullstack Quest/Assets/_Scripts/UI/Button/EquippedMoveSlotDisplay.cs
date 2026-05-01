using Shared.DataTransferObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedMoveSlotDisplay : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private TextMeshProUGUI _tmp;
    [SerializeField]
    private AudioClip _dropSound;

    private Hero _hero;
    private int _slotIndex;

    public MoveDto Move { get; private set; }

    public void OnDrop(PointerEventData eventData)
    {
        DropAction(eventData);
    }

    public void Initialize(Hero hero, MoveDto move, int slotIndex)
    {
        Move = move;
        _tmp.SetText(move.Name);

        _hero = hero;
        _slotIndex = slotIndex;
    }

    protected virtual void DropAction(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null) return;
        var move = eventData.pointerDrag.GetComponent<UnequippedMoveDisplay>().Move;

        Initialize(_hero, move, _slotIndex);
        _hero.Moves[_slotIndex] = move;
        AudioSource.PlayClipAtPoint(_dropSound, Camera.main.transform.position);

    }
}
