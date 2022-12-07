using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drives a healthbar given a health target.
/// </summary>
public class Healthbar : MonoBehaviour
{
    public Health target;
    public Image fill;
    public Image smoothedFill;
    public TMPro.TMP_Text healthText;
    public float smoothTime;

    [Space]
    public Vector2 offset;
    public float shakeAmplitude;
    public AnimationCurve shakeCurve;
    public float shakeFrequency;

    float fillVelocity;
    float shakeTime = float.MinValue;

    private void OnEnable()
    {
        target.DamageEvent += OnDamageEvent;
    }

    private void OnDisable()
    {
        target.DamageEvent -= OnDamageEvent;
    }

    private void OnDamageEvent(DamageArgs args)
    {
        shakeTime = Time.time;
    }

    private void LateUpdate()
    {
        UpdateFill();
        ShakeHealthbar();
    }

    /// <summary>
    /// Updates the visuals of the fill based on the current health.
    /// </summary>
    private void UpdateFill()
    {
        float healthPercent = target.CurrentHeath / target.MaxHealth;

        fill.fillAmount = healthPercent;
        smoothedFill.fillAmount = Mathf.SmoothDamp(smoothedFill.fillAmount, healthPercent, ref fillVelocity, smoothTime);

        healthText.text = $"{Mathf.RoundToInt(target.CurrentHeath)}/{Mathf.RoundToInt(target.MaxHealth)}";
    }

    /// <summary>
    /// Applys shake to the controlling ui.
    /// </summary>
    private void ShakeHealthbar()
    {
        RectTransform transform = this.transform as RectTransform;

        transform.anchoredPosition = offset;

        float angle = Mathf.PerlinNoise(Time.time * shakeFrequency, 0.5f) * Mathf.PI * 2.0f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        transform.anchoredPosition += direction * shakeCurve.Evaluate(Time.time - shakeTime) * shakeAmplitude;
    }
}
