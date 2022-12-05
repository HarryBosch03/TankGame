using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth = 100.0f;
    public float maxHealth = 100.0f;
    public float hitInvulnerability = -1.0f;

    [Space]
    public Transform[] detachOnDeath;

    [Space]
    public GameObject damagePrefab;
    public GameObject deathPrefab;

    [Space]
    [SerializeField] bool destroyOnDeath;

    float lastDamageTime;

    public event System.Action<DamageArgs> DamageEvent;
    public event System.Action<DamageArgs> DeathEvent;

    public float CurrentHeath => currentHealth;
    public float MaxHealth => maxHealth;

    public void Damage(DamageArgs args)
    {
        if (lastDamageTime + hitInvulnerability > Time.time) return;

        currentHealth -= args.damage;

        if (damagePrefab) Instantiate(damagePrefab, transform.position, Quaternion.identity);

        lastDamageTime = Time.time;

        if (currentHealth <= 0.0f)
        {
            args.damage += currentHealth;
            DamageEvent?.Invoke(args);

            Die(args);
        }
        else
        {
            DamageEvent?.Invoke(args);
        }
    }

    public void Die(DamageArgs args)
    {
        currentHealth = 0.0f;

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (deathPrefab)
        {
            GameObject deathInstance = Instantiate(deathPrefab, transform.position, Quaternion.identity);
            if (rigidbody && deathInstance.TryGetComponent(out Rigidbody2D diRigidbody))
            {
                diRigidbody.velocity = rigidbody.velocity;
            }
        }

        foreach (var detach in detachOnDeath)
        {
            if (!detach) continue;

            detach.SetParent(null);
            if (rigidbody && detach.TryGetComponent(out Rigidbody2D detachRigidbody))
            {
                detachRigidbody.velocity = rigidbody.velocity;
            }
        }

        DeathEvent?.Invoke(args);

        if (destroyOnDeath) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    public void Regen(DamageArgs args)
    {
        currentHealth = Mathf.Min(currentHealth + args.damage, MaxHealth);
    }
}

public struct DamageArgs
{
    public GameObject damager;
    public float damage;

    public DamageArgs(GameObject damager, float damage)
    {
        this.damager = damager;
        this.damage = damage;
    }
}
