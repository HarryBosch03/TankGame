using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchscreenStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 offset;
    public bool useSoftZone;
    public float startSoftZone;

    [Space]
    public UnityEvent<Vector2> ValueChangeEvent;
    public UnityEvent StickDownEvent;
    public UnityEvent StickUpEvent;

    Vector2 vector;

    new public RectTransform transform => base.transform as RectTransform;
    public RectTransform child => transform.GetChild(0) as RectTransform;

    private void Awake()
    {
        offset = transform.anchoredPosition;
    }

    private void Update()
    {
        gameObject.SetActive(Application.platform == RuntimePlatform.Android);
        ValueChangeEvent?.Invoke(vector);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if ((eventData.position - (Vector2)child.position).sqrMagnitude < startSoftZone * startSoftZone && useSoftZone)
        {
            transform.position = eventData.position;
        }
        else
        {
            child.position = eventData.position;
        }

        StickDownEvent?.Invoke();

        eventData.Use();
    }

    public void OnDrag(PointerEventData eventData)
    {
        child.position = eventData.position;
        child.anchoredPosition = Vector2.ClampMagnitude(child.anchoredPosition, Mathf.Min(transform.rect.width, transform.rect.height) / 2.0f);

        vector = child.anchoredPosition / transform.rect.size * 2.0f;

        eventData.Use();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (useSoftZone) transform.anchoredPosition = offset;
        child.anchoredPosition = Vector2.zero;

        vector = Vector2.zero;

        StickUpEvent?.Invoke();

        eventData.Use();
    }
}
