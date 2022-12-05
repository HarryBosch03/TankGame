using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleFlipbook : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float speed;

    SpriteRenderer display;

    private void Awake()
    {
        display = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        display.sprite = sprites[(int)(Time.time * speed) % sprites.Length];
    }
}
