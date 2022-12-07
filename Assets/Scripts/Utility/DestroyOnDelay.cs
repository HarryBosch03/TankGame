using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I think you can figure this one out.
/// </summary>
public class DestroyOnDelay : MonoBehaviour
{
    public float delay;

    private void OnEnable()
    {
        Destroy(gameObject, delay);
    }
}
