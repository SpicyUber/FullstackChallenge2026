using Shared.DataTransferObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnequippedMoveDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI _tmp;

    public MoveDto Move { get; private set; }
    private MoveDetailsDisplay _detailsDisplay;

    private Canvas _canvas;

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

    public void SetMoveAndDetailsPanel(MoveDto move, MoveDetailsDisplay detailsDisplay)
    {
        Move = move;
        _tmp.SetText(move.Name);

        _detailsDisplay = detailsDisplay;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _detailsDisplay.Display(Move);
    }
}
