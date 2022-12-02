using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpriteShadow : MonoBehaviour
{
    public Vector2 offset;

    public Vector2 Offset { get => offset; set => offset = value; }

    private void OnEnable()
    {
        offset = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (transform.parent)
        {
            transform.position = (Vector2)transform.parent.position + offset;
            transform.rotation = transform.parent.rotation;
        }
    }
}
