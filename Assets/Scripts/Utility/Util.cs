using UnityEngine;

/// <summary>
/// Libary of helper functions made by yours truly <3.
/// </summary>
public static class Util
{
    /// <summary></summary>
    /// <param name="spawnRange">The range to look</param>
    /// <param name="spawnCheckRadius">The size of box to use for collision checking</param>
    /// <returns>Returns a Spawn Location within a specified range</returns>
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
