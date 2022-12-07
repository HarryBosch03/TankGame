using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Used to animate and move an object as the cursor.
/// </summary>
public class TankCursor : MonoBehaviour
{
    public SpriteRenderer cursor;

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = !focus;
    }

    private void LateUpdate()
    {
#if !UNITY_EDITOR
        gameObject.SetActive(Application.platform != RuntimePlatform.Android);
#endif

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}
