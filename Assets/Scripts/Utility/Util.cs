using UnityEngine;

public static class Util
{
    public static Vector2 GetSpawnLocation(float spawnRange, float spawnCheckRadius)
    {
        Vector2 spawnLocation;
        do
        {
            float angle = Random.value * Mathf.PI * 2.0f;
            float dist = Random.Range(0.0f, spawnRange);
            spawnLocation = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
        }
        while (Physics2D.OverlapBox(spawnLocation, Vector2.one * spawnCheckRadius, 0.0f));
        return spawnLocation;
    }
}
