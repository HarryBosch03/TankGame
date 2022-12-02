using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDIsplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float scaleFrequency;
    [SerializeField] float scaleAmplitude;
    [SerializeField] bool lockDirection;

    private void LateUpdate()
    {
        if (lockDirection)
        {
            sprite.transform.rotation = Quaternion.identity;
        }

        sprite.transform.localScale = Vector3.one * Mathf.Exp(Mathf.PerlinNoise(Time.time * scaleFrequency, 0.5f) * scaleAmplitude);
    }
}
