using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Spawn Profile")]
public class EnemySpawnProfile : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] float weight;
    [SerializeField][EditorTime] float minTime;
    [SerializeField] bool ignore;

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public float Weight { get => weight; set => weight = value; }
    public float MinTime { get => minTime; set => minTime = value; }

    public static List<EnemySpawnProfile> Profiles { get; } = new List<EnemySpawnProfile>();
    public static List<WeightedElement<EnemySpawnProfile>> WeightedProfiles { get; } = new List<WeightedElement<EnemySpawnProfile>>();

    private void Awake()
    {
        Profiles.Add(this);
        WeightedProfiles.Add(new WeightedElement<EnemySpawnProfile>(this, weight));
    }
}
