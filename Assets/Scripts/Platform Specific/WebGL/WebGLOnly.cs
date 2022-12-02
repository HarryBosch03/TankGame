using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLOnly : MonoBehaviour
{
    public bool invert;

    private void OnEnable()
    {
        gameObject.SetActive(Application.platform == RuntimePlatform.WebGLPlayer != invert);
    }
}
