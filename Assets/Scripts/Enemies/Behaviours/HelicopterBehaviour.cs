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
            state = "Move";
            yield return StartCoroutine(MoveToPointAroundTarget());
            state = "Wait";
            foreshadowEvent?.Invoke();
            yield return new WaitForSeconds(waitTime);
            state = "Shoot";
            yield return StartCoroutine(ShootTarget());
        }
    }

    private IEnumerator MoveToPointAroundTarget()
    {
        float angle = Random.value * Mathf.PI * 2.0f;
        Vector2 point = (Vector2)Target.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

        while ((point - (Vector2)transform.position).sqrMagnitude > 1.0f)
        {
            MoveTowards(point);

            Vector2 vector = (Vector2)Target.transform.position - point;
            heliMovement.Rotation = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            yield return null;
        }
    }

    private IEnumerator ShootTarget()
    {
        float time = 0.0f;
        while (time < shootTime)
        {
            Attack(0);
            time += Time.deltaTime;
            yield return null;
        }
    }

    protected override void MoveInDirection(Vector2 vector)
    {
        heliMovement.MoveInput = vector.normalized;
    }
}
