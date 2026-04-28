using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2Int _lastScreenSize;

    [Header("Margins (Pixels)")]
    [SerializeField] private Vector2 _marginMin; 
    [SerializeField] private Vector2 _marginMax; 

    [Header("Margins (% of screen)")]
    [SerializeField] private Vector2 _marginMinPercent; 
    [SerializeField] private Vector2 _marginMaxPercent;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        ApplyMargins();
    }

    private void Update()
    {
        if (Screen.width != _lastScreenSize.x ||
            Screen.height != _lastScreenSize.y)
        {
            ApplyMargins();
        }
    }

    private void ApplyMargins()
    {
        _lastScreenSize = new Vector2Int(Screen.width, Screen.height);

        float width = Screen.width;
        float height = Screen.height;

        
        float left = _marginMin.x + _marginMinPercent.x * width;
        float bottom = _marginMin.y + _marginMinPercent.y * height;
        float right = _marginMax.x + _marginMaxPercent.x * width;
        float top = _marginMax.y + _marginMaxPercent.y * height;

        Rect safeArea = new Rect(
            left,
            bottom,
            width - left - right,
            height - bottom - top
        );

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= width;
        anchorMin.y /= height;
        anchorMax.x /= width;
        anchorMax.y /= height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }
}