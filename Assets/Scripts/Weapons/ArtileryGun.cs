using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Used for a delayed, non moving projectile spawning at a location.
/// </summary>
public class ArtileryGun : MonoBehaviour, IAttack
{
    public GameObject explosionPrefab;
    public float fireDelay;
    public float accuracy;
    public Transform targetPoint;
    public UnityEvent fireEvent;

    float lastFireTime;

    public float Cooldown => throw new System.NotImplementedException();

    /// <summary>
    /// Spawns the explosion prefab at the target.
    /// </summary>
    /// <param name="input"></param>
    public void Shoot (float input)
    {
        if (input > 0.5f && Time.time > lastFireTime + fireDelay)
        {
            Vector2 point = (Vector2)targetPoint.position + Random.insideUnitCircle * accuracy;
            Instantiate(explosionPrefab, point, Quaternion.identity);

            lastFireTime = Time.time;

            fireEvent?.Invoke();
        }
    }
}
