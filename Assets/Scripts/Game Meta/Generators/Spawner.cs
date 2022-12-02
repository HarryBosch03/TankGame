using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float delay;
    public GameObject spawnObject;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(spawnObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
