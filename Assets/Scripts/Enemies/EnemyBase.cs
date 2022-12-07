using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    const float pathfindingDelay = 1.0f;

    [Space]
    public bool pathfind = true;
    public float pathCheckDistance;

    [Space]
    public float angleScale = 0.05f;
    public UnityEvent[] attackEvents;
    public bool canFireOffScreen;

    [Space]
    public Transform muzzle;
    public Transform targetPoint;

    [Space]
    public Team[] enemyTeams;

    protected TankMovement movement;

    private GameObject _Target;
    private Health health;
    private Vector2 movePoint;

    private float lastPathfindTime = float.MinValue;
    private List<Vector2> path = new List<Vector2>();

    public Vector2 ShootPoint { get => movement ? movement.ShootPoint : Vector2.zero; set { if (movement) movement.ShootPoint = value; } }
    public Vector2 MuzzleDirection => muzzle.right;

    public GameObject Target
    {
        get
        {
            if (!_Target)
            {
                var enemies = Team.GetOverlapingSet(enemyTeams);
                if (enemies.Count > 0)
                {
                    _Target = enemies.ElementAt(Random.Range(0, enemies.Count)).gameObject;
                }
            }
            return _Target;
        }
        set => _Target = value;
    }

    public static HashSet<EnemyBase> Enemies { get; } = new HashSet<EnemyBase>();

    protected virtual void Awake()
    {
        movement = GetComponent<TankMovement>();
    }

    private void OnEnable()
    {
        Enemies.Add(this);

        if (TryGetComponent(out health))
        {
            health.DamageEvent += OnDamageEvent;
        }
    }

    private void OnDamageEvent(DamageArgs args)
    {
        if (args.damager) Target = args.damager;
    }

    private void LateUpdate()
    {
        Vector2 point = transform.position;
        Vector2 direction = movePoint - (Vector2)transform.position;
        float distance = direction.magnitude;
        direction /= distance;

        if (distance < 1.0f)
        {
            MoveInDirection(Vector2.zero);
        }
        else if (!pathfind)
        {
            MoveInDirection(direction);
        }
        else if (Physics2D.CircleCast(point, 1.0f, direction, distance, 1))
        {
            if (Time.time > lastPathfindTime + pathfindingDelay)
            {
                Pathfinder.Instance.GetPath(transform.position, movePoint, path);
                lastPathfindTime = Time.time;
            }

            while (path.Count > 0 && (path.First() - (Vector2)transform.position).sqrMagnitude < pathCheckDistance * pathCheckDistance)
            {
                path.RemoveAt(0);
            }

            if (path.Count > 0)
            {
                MoveInDirection(path.First() - (Vector2)transform.position);
            }
            else
            {
                MoveInDirection(Vector2.zero);
            }
        }
        else
        {
            MoveInDirection(direction);
        }
    }

    private void OnDisable()
    {
        Enemies.Remove(this);

        health.DamageEvent -= OnDamageEvent;
    }

    protected virtual void Update()
    {
        if (Target)
        {
            ShootPoint = Target.transform.position;
            targetPoint.position = Target.transform.position;
        }
    }

    public void Attack(int index)
    {
        if (index < 0 || index >= attackEvents.Length) return;

        if (!IsOnScreen() && !canFireOffScreen) return;

        attackEvents[index].Invoke();
    }

    private bool IsOnScreen()
    {
        var sp = Camera.main.WorldToViewportPoint(transform.position);
        return sp.x > 0.0f && sp.x < 1.0f && sp.y > 0.0f && sp.y < 1.0f;
    }

    public void MoveTowards(Vector2 point)
    {
        movePoint = point;
    }

    protected virtual void MoveInDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f * 0.01f)
        {
            movement.ThrottleInput = 0.0f;
            movement.TurnInput = 0.0f;
            return;
        }

        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = movement.transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);

        float dot = Vector2.Dot(direction, movement.transform.right);

        movement.ThrottleInput = dot;
        movement.TurnInput = Mathf.Clamp(deltaAngle * angleScale, -1.0f, 1.0f);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < path.Count; i++)
        {
            if (i != path.Count - 1) Gizmos.DrawLine(path[i], path[i + 1]);

            Gizmos.DrawSphere(path[i], 0.1f);
        }

        Gizmos.color = Color.white;
    }
}
