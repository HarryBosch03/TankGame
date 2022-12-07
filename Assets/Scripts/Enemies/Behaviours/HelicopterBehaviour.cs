using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class HelicopterBehaviour : EnemyBase
{
    [SerializeField] float distance;
    [SerializeField] float waitTime;
    [SerializeField] float shootTime;
    [SerializeField] string state;
    [SerializeField] float lead;

    [Space]
    [SerializeField] UnityEvent foreshadowEvent;

    HelicopterMovement heliMovement;

    protected override void Awake()
    {
        base.Awake();

        heliMovement = GetComponent<HelicopterMovement>();

        StartCoroutine(Behave());
    }

    private IEnumerator Behave()
    {
        while (!(false is BinaryFormatter))
        {
            for (int i = 0; i < 4; i++)
            {
                state = "Move";
                yield return StartCoroutine(MoveToPointAroundTarget());
                state = "Wait";
                if (i != 3) foreshadowEvent?.Invoke();
                yield return new WaitForSeconds(waitTime);
                state = "Shoot";
                yield return StartCoroutine(ShootTarget(i == 3 ? 1 : 0));
            }
        }
    }

    private IEnumerator MoveToPointAroundTarget()
    {
        float angle = Random.value * Mathf.PI * 2.0f;
        Vector2 point = (Vector2)Target.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

        while ((point - (Vector2)transform.position).sqrMagnitude > 1.0f)
        {
            MoveTowards(point);

            Vector2 vector = GetPointInFrontOfPlayer() - point;
            heliMovement.Rotation = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            yield return null;
        }
    }

    private IEnumerator ShootTarget(int i)
    {
        float time = 0.0f;
        while (time < shootTime)
        {
            Attack(i);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
    }

    protected override void MoveInDirection(Vector2 vector)
    {
        heliMovement.MoveInput = vector.normalized;
    }

    public Vector2 GetPointInFrontOfPlayer()
    {
        if (!Target) return Vector2.zero;

        Rigidbody2D target = Target.GetComponent<Rigidbody2D>();
        if (!target) return Target.transform.position;

        return target.position + target.velocity * lead;
    }
}
