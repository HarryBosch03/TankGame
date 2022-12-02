using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArtileryGun : MonoBehaviour, IAttack
{
    public GameObject explosionPrefab;
    public float fireDelay;
    public Transform targetPoint;
    public UnityEvent fireEvent;

    float lastFireTime;

    public float Cooldown => throw new System.NotImplementedException();

    public void Shoot (float input)
    {
        if (input > 0.5f && Time.time > lastFireTime + fireDelay)
        {
            Instantiate(explosionPrefab, targetPoint.position, Quaternion.identity);

            lastFireTime = Time.time;

            fireEvent?.Invoke();
        }
    }
}
