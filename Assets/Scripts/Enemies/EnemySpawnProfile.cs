using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Spawn Profile")]
public class EnemySpawnProfile : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] float weight;
    [SerializeField][EditorTime] float minTime;

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public float Weight { get => weight; set => weight = value; }
    public float MinTime { get => minTime; set => minTime = value; }
}
