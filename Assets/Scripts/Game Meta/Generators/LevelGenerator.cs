using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap target;
    public Vector2Int mapSize;
    public TerrainFeature[] features;

    [Space]
    public TileBase wallTile;
    public float wallRange;
    public float wallNoiseScale;
    public float wallNoiseAmplitude;

    private void Start()
    {
        GenerateMap();
    }

    [ContextMenu("Generate Map")]
    private void GenerateMap()
    {
        target.ClearAllTiles();

        HashSet<Vector2Int> openArea = new HashSet<Vector2Int>();

        for (int x = -mapSize.x; x <= mapSize.x; x++)
        {
            for (int y = -mapSize.y; y <= mapSize.y; y++)
            {
                float distance = new Vector2(x, y).magnitude / wallRange;
                float boundry = distance + Mathf.PerlinNoise((x + mapSize.x) * wallNoiseScale, (y + mapSize.y) * wallNoiseScale) * wallNoiseAmplitude;
                if (boundry < 1.0f)
                {
                    openArea.Add(new Vector2Int(x, y));
                }
            }
        }

        Queue<Vector2Int> openList = new Queue<Vector2Int>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

        openList.Enqueue(new Vector2Int(0, 0));
        while (openList.Count > 0)
        {
            Vector2Int pos = openList.Dequeue();
            if (closedList.Contains(pos)) continue;

            closedList.Add(pos);

            foreach (Vector2Int offset in Neighbours)
            {
                if (openArea.Contains(pos + offset))
                {
                    openList.Enqueue(pos + offset);
                }
            }
        }

        for (int x = -mapSize.x; x <= mapSize.x; x++)
        {
            for (int y = -mapSize.y; y <= mapSize.y; y++)
            {
                if (!closedList.Contains(new Vector2Int(x, y)))
                {
                    target.SetTile(new Vector3Int(x, y, 0), wallTile);
                }

                foreach (TerrainFeature feature in features)
                {
                    if (feature.IsValidHere(target, x, y))
                    {
                        target.SetTile(new Vector3Int(x, y, 0), feature.tile);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class TerrainFeature
    {
        public TileBase tile;
        public bool replace;

        [Space]
        public float noiseScale;
        public float threshold;

        public bool IsValidHere(Tilemap level, int x, int y)
        {
            if (!replace && level.GetTile(new Vector3Int(x, y, 0)) != null) return false;
            if (Mathf.PerlinNoise(x * noiseScale + 10000, y * noiseScale + 10000) < threshold) return false;

            return true;
        }
    }

    public static readonly Vector2Int[] Neighbours = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    };
}
