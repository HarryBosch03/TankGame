using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform muzzle;
    public UnityEvent fireEvent;

    public void Shoot ()
    {
        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        fireEvent?.Invoke();
    }
}
