using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Spawning/Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    public int minWaveIndex;
    public int maxWaveIndex;
    public List<GameObject> enemies;
    public float preDelay;
    public float tweenDelay;
}
