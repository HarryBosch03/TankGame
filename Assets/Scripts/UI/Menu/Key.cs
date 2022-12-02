using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public InputAction action;
    public Sprite onSprite;
    public Sprite offSprite;
    public Image target;

    private void Reset()
    {
        target = GetComponent<Image>();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Update()
    {
        target.sprite = action.ReadValue<float>() > 0.5f ? onSprite : offSprite;
    }
}
