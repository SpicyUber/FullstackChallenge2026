using UnityEngine;
using Shared.DataTransferObjects;
using TMPro;
using UnityEngine.EventSystems;

public class UnequippedItemDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI _tmp;

    public ItemDto Item { get; private set; }
    private ItemDetailsDisplay _detailsDisplay;

    private BaseCharacter _hero;
    private Canvas _canvas;

    private TextMeshProUGUI _tmpText;

    private RectTransform _rectTransform;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalPosition = _rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = _originalPosition;
    }

    public void SetItemAndDetailsPanel(ItemDto item, ItemDetailsDisplay detailsDisplay, BaseCharacter hero)
    {
        Item = item;
        _tmp.SetText(item.Name);

        _hero = hero;
        _detailsDisplay = detailsDisplay;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _detailsDisplay.Display(Item, _hero);
    }
}

