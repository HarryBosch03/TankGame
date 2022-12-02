using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    const float boxSize = 2.0f;
    static readonly Vector2Int start = new Vector2Int(-85, -85);
    static readonly Vector2Int end = new Vector2Int(85, 85);

    public Dictionary<Vector2Int, bool> map;

    static Pathfinder _instance;
    public static Pathfinder Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<Pathfinder>();
            }
            if (!_instance)
            {
                _instance = new GameObject("Pathfinder").AddComponent<Pathfinder>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        RegenerateMap();
    }

    [ContextMenu("Regenerate Map")]
    private void RegenerateMap()
    {
        LayerMask mask = LayerMask.GetMask("Default");
        map = new Dictionary<Vector2Int, bool>();

        int xw = (end.x - start.x) / (int)boxSize;
        int yw = (end.y - start.y) / (int)boxSize;

        for (int x = 0; x < xw; x++)
        {
            for (int y = 0; y < yw; y++)
            {
                Vector2Int key = new Vector2Int(x, y);
                Vector2 worldPos = start + (Vector2)key * boxSize;

                map.Add(key, Physics2D.OverlapBox(worldPos, Vector2.one * boxSize, 0.0f, mask));
            }
        }
    }

    public void GetPath(Vector2 startWorld, Vector2 endWorld, List<Vector2> path)
    {
        path.Clear();

        List<PathfindingNode> openNodes = new List<PathfindingNode>();
        Dictionary<Vector2Int, PathfindingNode> closedNodes = new Dictionary<Vector2Int, PathfindingNode>();

        Vector2Int start = Vector2Int.RoundToInt((startWorld - Pathfinder.start) / boxSize);
        Vector2Int end = Vector2Int.RoundToInt((endWorld - Pathfinder.start) / boxSize);
        PathfindingNode head = null;

        start = GetClosesetValidPoint(start);
        end = GetClosesetValidPoint(end);

        openNodes.Add(new PathfindingNode(start, start, end, null));

        while (openNodes.Count > 0)
        {
            PathfindingNode node = openNodes[0];
            int index = 0;
            for (int i = 0; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost < node.fCost)
                {
                    node = openNodes[i];
                    index = i;
                }
            }

            openNodes.RemoveAt(index);
            closedNodes.Add(node.key, node);

            if (node.key == end)
            {
                head = node;
                break;
            }

            foreach (var offset in Neighbours)
            {
                if (!map.ContainsKey(node.key + offset)) continue;
                if (map[node.key + offset]) continue;

                PathfindingNode tmp = openNodes.Find(q => q.key == node.key + offset);
                if (tmp != null)
                {
                    tmp.previous = node;
                }
                else if (!closedNodes.ContainsKey(node.key + offset))
                {
                    openNodes.Add(new PathfindingNode(node.key + offset, start, end, node));
                }

                //Debug.DrawLine((Vector2)node.key * boxSize + Pathfinder.start, ((Vector2)node.key + offset) * boxSize + Pathfinder.start, Color.yellow);
            }
        }

        if (head == null)
        {
            path.Add(end);
            return;
        }

        Vector2Int direction = head.previous != null ? head.key - head.previous.key : Vector2Int.zero;

        head = head.previous;
        path.Add(endWorld);

        while (head?.previous != null)
        {
            if (head.key - head.previous.key != direction)
            {
                //Debug.DrawLine(Pathfinder.start + (Vector2)head.key * boxSize, Pathfinder.start + (Vector2)head.previous.key * boxSize, Color.green);

                path.Add((Vector2)head.key * boxSize + Pathfinder.start);
                direction = head.key - head.previous.key;
            }

            head = head.previous;
        }

        path.Add(startWorld);

        path.Reverse();
    }

    private Vector2Int GetClosesetValidPoint(Vector2Int start)
    {
        Vector2Int point = start;

        List<Vector2Int> openList = new List<Vector2Int>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> previous = new Dictionary<Vector2Int, Vector2Int>();

        openList.Add(point);

        while (map[point])
        {
            point = openList[0];
            int index = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (CloseishDistance(openList[i] - start) < CloseishDistance(point - start))
                {
                    point = openList[i];
                    index = i;
                }
            }

            openList.RemoveAt(index);
            closedList.Add(point);

            foreach (var offset in Neighbours)
            {
                int i = openList.FindIndex(q => q == point + offset);
                if (openList.Exists(q => q == point + offset) && previous.ContainsKey(point + offset))
                {
                    previous[openList[i]] = point + offset;
                }
                else if (!closedList.Contains(point + offset) && map.ContainsKey(point + offset))
                {
                    openList.Add(point + offset);
                }
            }
        }
        return point;
    }

    private void OnDrawGizmosSelected()
    {
        if (map == null) RegenerateMap();

        foreach (var pair in map)
        {
            Gizmos.color = pair.Value ? Color.green : Color.red;

            Vector2 worldPos = start + (Vector2)pair.Key * boxSize;
            Gizmos.DrawWireCube(worldPos, Vector2.one * boxSize * 0.9f);
        }

        Gizmos.color = Color.white;
    }

    public class PathfindingNode
    {
        public Vector2Int key;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        public PathfindingNode previous;

        public PathfindingNode(Vector2Int key, Vector2Int start, Vector2Int end, PathfindingNode previous)
        {
            this.key = key;
            gCost = CloseishDistance(start - key);
            hCost = CloseishDistance(end - key);
            this.previous = previous;
        }
    }

    public static int CloseishDistance(Vector2Int v)
    {
        v.x = Mathf.Abs(v.x);
        v.y = Mathf.Abs(v.y);

        int min = Mathf.Min(v.x, v.y);
        int max = Mathf.Max(v.x, v.y);

        return (max - min) * 10 + min * 14;
    }

    readonly Vector2Int[] Neighbours = new Vector2Int[]
    {
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  0),
        new Vector2Int( 0,  1),
        new Vector2Int( 0, -1),
    };
}
