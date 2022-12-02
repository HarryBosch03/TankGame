using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AimZone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEvent downEvent;
    public UnityEvent upEvent;
    public UnityEvent<Vector2> dragEvent;

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        dragEvent?.Invoke(eventData.position);

        eventData.Use();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragEvent?.Invoke(eventData.position);
        upEvent?.Invoke();

        eventData.Use();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragEvent?.Invoke(eventData.position);
        downEvent?.Invoke();

        eventData.Use();
    }
}
