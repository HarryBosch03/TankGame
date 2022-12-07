using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatContributor : MonoBehaviour
{
    public TankGun mainGun;

    float startTime;

    private void OnEnable()
    {
        mainGun.ShootEvent += OnShootEvent;
        mainGun.HitEvent += OnHitEvent;
    }

    private void OnDisable()
    {
        mainGun.ShootEvent -= OnShootEvent;
        mainGun.HitEvent -= OnHitEvent;
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {   
        Stats.Main.timeAlive.Value = Time.time - startTime;
    }

    private void OnShootEvent()
    {
        Stats.Main.shotsFired.Value++;
    }

    private void OnHitEvent(GameObject hitObject, DamageArgs args)
    {
        Stats.Main.damageDelt.Value += args.damage;

        if (hitObject.TryGetComponent(out Health health))
        {
            if (health.currentHealth <= 0)
            {
                Stats.Main.tanksDestroyed.Value++;
                ScoreCalculator.AwardKillPoints(health);
            }
        }
    }
}
