using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TankGun : MonoBehaviour, IAttack
{
    public GameObject projectilePrefab;
    public Transform muzzle;
    public float fireDelay;
    public bool fullAuto;

    [Space]
    public UnityEvent shootEventEditor;

    [Space]
    public Transform turretSprite;
    public float distortionDuration;
    public AnimationCurve xScale;
    public AnimationCurve yScale;

    float nextFireTime;
    bool triggerState;

    public event System.Action ShootEvent;
    public event System.Action<GameObject, DamageArgs> HitEvent;

    public float FireDelay => fireDelay;
    public float NextFireTime => nextFireTime;
    public float Cooldown => 1.0f - Mathf.Clamp01((nextFireTime - Time.time) / fireDelay);

    private void OnEnable()
    {
        //nextFireTime = Time.time + fireDelay;
    }

    private void Update()
    {
        if (Time.time < nextFireTime) return;
        if (!triggerState) return;

        GameObject projectileObject = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        if (projectileObject.TryGetComponent(out Projectile projectile))
        {
            projectile.hitEvent.AddListener(OnHitEvent);
            projectile.Shooter = transform.GetComponentInParent<Health>();
        }

        nextFireTime = Time.time + fireDelay;

        ShootEvent?.Invoke();
        shootEventEditor?.Invoke();

        if (!fullAuto) triggerState = false;
    }

    private void LateUpdate()
    {
        float t = (Time.time - (nextFireTime - FireDelay)) / distortionDuration;
        if (turretSprite && t > 0.0f && t < 1.0f)
        {
            turretSprite.localScale = new Vector3(xScale.Evaluate(t), yScale.Evaluate(t), 1.0f);
        }
    }

    public void Shoot(float inputValue)
    {
        triggerState = inputValue > 0.5f;
    }

    public void OnHitEvent (GameObject hitObject, DamageArgs args)
    {
        HitEvent?.Invoke(hitObject, args);
    }
}
