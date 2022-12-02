using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileOnly : MonoBehaviour
{
    public bool invert;

    private void OnEnable()
    {
        gameObject.SetActive((Application.platform == RuntimePlatform.Android) == !invert);
    }
}
