using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Spawn Profile")]
public class EnemySpawnProfile : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] AnimationCurve weight;

    public GameObject Prefab { get => prefab; set => prefab = value; }

    public float GetWeight(float gameTimeSecconds) => weight.Evaluate(gameTimeSecconds);
}
