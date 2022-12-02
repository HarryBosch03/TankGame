using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchscreenWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public PlayerController target;
    public float rockAmount;
    public float smoothTime;

    Vector2 rock;
    Vector2 rockVelocity;

    new public RectTransform transform => base.transform as RectTransform;

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(rock.y * rockAmount, rock.x * rockAmount, 0.0f);
        rock = Vector2.SmoothDamp(rock, new Vector2(target.Steering, target.Throttle), ref rockVelocity, smoothTime);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ProcessInput(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ProcessInput(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        target.Throttle = 0.0f;
        target.Steering = 0.0f;
    }

    private void ProcessInput(Vector2 position)
    {
        Vector2 direction = (position - (Vector2)transform.position).normalized;

        float throttleAngle = Mathf.Acos(Vector2.Dot(direction, Vector2.up)) * Mathf.Rad2Deg;
        float steeringAngle = Mathf.Acos(Vector2.Dot(direction, Vector2.right)) * Mathf.Rad2Deg;

        target.Throttle = 0.0f;
        target.Steering = 0.0f;

        print(throttleAngle);

        if (throttleAngle < 67.5f) target.Throttle = 1.0f;
        if (throttleAngle > 112.5f) target.Throttle = -1.0f;

        if (steeringAngle < 67.5f) target.Steering = 1.0f;
        if (steeringAngle > 112.5f) target.Steering = -1.0f;
    }
}
