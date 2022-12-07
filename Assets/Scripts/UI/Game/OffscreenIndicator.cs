using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for the heads that pop up when an object of interest is off the screen.
/// </summary>
public class OffscreenIndicator : MonoBehaviour
{
    public float cameraRectShrinkage;
    public float scaleSmoothTime;

    float scalePercent;
    float scaleVelocity;

    private void LateUpdate()
    {
        Camera camera = Camera.main;

        float width = camera.orthographicSize * Screen.width / Screen.height - cameraRectShrinkage;
        float height = camera.orthographicSize - cameraRectShrinkage;
        Bounds bounds = new Bounds
        {
            extents = new Vector3(width, height),
            center = (Vector2)camera.transform.position,
        };

        transform.position = bounds.ClosestPoint(transform.parent.position);

        bool onScreen = bounds.Contains(transform.parent.position);
        scalePercent = Mathf.SmoothDamp(scalePercent, onScreen ? 0.0f : 1.0f, ref scaleVelocity, scaleSmoothTime);

        transform.localScale = Vector3.one * scalePercent;
        transform.rotation = Quaternion.identity;
    }
}
