using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtileryStrike : MonoBehaviour
{
    public float damage;
    public AnimationCurve damageFalloff;
    public float range;
    public LayerMask damageMask;
    public int rays;
    public float delay;
    public Transform fxDetach;

    private void Start()
    {
        fxDetach.SetParent(null);

        StartCoroutine(Explosion());
    }

    /// <summary>
    /// Deals explosion damage after a short delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(delay);

        HashSet<Health> damaged = new HashSet<Health>();
        for (int i = 0; i < rays; i++)
        {
            float angle = (i / (float)rays) * 2.0f * Mathf.PI;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            Physics2D.queriesStartInColliders = true;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, range, damageMask);
            Physics2D.queriesStartInColliders = false;

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Health health))
                {
                    if (!damaged.Contains(health))
                    {
                        float normalizedDistance = hit.distance / range;
                        health.Damage(new DamageArgs(transform.root.gameObject, damage * damageFalloff.Evaluate(normalizedDistance)));
                        damaged.Add(health);
                    }
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < rays; i++)
        {
            float angle = (i / (float)rays) * 2.0f * Mathf.PI;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, damageMask);
            if (hit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, hit.point);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, direction * range);
            }
        }

        Gizmos.color = Color.white;
    }
}
