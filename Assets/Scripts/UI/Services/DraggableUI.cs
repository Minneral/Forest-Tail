using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Vector2 _originalPosition;

    private InventorySlotUI _myType; // Ссылка на компонент MyType

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        // Попытка получить компонент MyType
        _myType = GetComponent<InventorySlotUI>();

        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Проверяем, имеет ли объект компонент MyType
        if (_myType == null)
        {
            Debug.LogWarning("Этот объект нельзя перетаскивать!");
            return;
        }

        _originalPosition = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_myType == null || _canvas == null) return;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );
        _rectTransform.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_myType == null) return;

        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        // Если нужно вернуть объект в исходное положение:
        _rectTransform.anchoredPosition = _originalPosition;
    }
}
