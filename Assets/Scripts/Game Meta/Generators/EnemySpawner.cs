using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Spawner spawnerPrefab;
    public List<WeightedElement<EnemyBase>> enemies;
    public float spawnRange;
    public float spawnCheckRange;

    [Space]
    public int minEnemies;
    public int maxEnemies;
    public float spawnPreDelay;
    public float spawnCountScalar;
    public float spawnCountOffset;

    [Space]
    public Team enemyTeam;
    public Signal enemyDiedEvent;

    float gameDuration;
    float nextSpawnTime;

    bool killedDummyTanks = false;
    bool spawningNewWave;

    float time;

    private void OnEnable()
    {
        enemyDiedEvent.OnRaise += TrySpawnWave;
    }

    private void OnDisable()
    {
        enemyDiedEvent.OnRaise -= TrySpawnWave;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void TrySpawnWave()
    {
        StartCoroutine(TrySpawnWaveRoutine());
    }

    private IEnumerator TrySpawnWaveRoutine()
    {
        if (spawningNewWave) yield break;

        if (enemyTeam.players.Count >= minEnemies) yield break;

        spawningNewWave = true;

        yield return new WaitForSeconds(spawnPreDelay);

        int enemiesToSpawn = Mathf.Min((int)(time * spawnCountScalar + spawnCountOffset), maxEnemies);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 spawnLocation = Util.GetSpawnLocation(spawnRange, spawnCheckRange);
            var spawner = Instantiate(spawnerPrefab, spawnLocation, Quaternion.identity);
            spawner.spawnObject = enemies.Evaluate().gameObject;

            yield return new WaitForSeconds(1.0f);
        }

        spawningNewWave = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        Gizmos.color = Color.white;
    }
}
