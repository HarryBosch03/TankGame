using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float hitForce;
    public float startSpeed;
    public float projectileSize;
    public LayerMask collisionMask;
    public float range;
    public AnimationCurve scaleCurve;

    [Space]
    public SpriteShadow shadow;
    public AnimationCurve shadowDistance;

    [Space]
    public GameObject landFX;
    public GameObject impactFX;

    [Space]
    public UnityEvent landEvent;
    public UnityEvent<GameObject, DamageArgs> hitEvent;

    float age;
    new Rigidbody2D rigidbody;
    Vector2 previousPosition;

    public Health Shooter { get; set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = transform.right * startSpeed;
    }

    private void Start()
    {
        previousPosition = rigidbody.position;
    }

    private void FixedUpdate()
    {
        Vector2 point = previousPosition;
        Vector2 vector = rigidbody.position + rigidbody.velocity * Time.deltaTime - previousPosition;
        float distance = vector.magnitude;
        Vector2 direction = vector / distance;

        var hits = Physics2D.CircleCastAll(point, projectileSize, direction, distance + 0.1f);
        foreach (var hit in hits)
        {
            DamageArgs args = new DamageArgs(Shooter ? Shooter.gameObject : null, damage);
            var health = hit.transform.GetComponentInParent<Health>();

            if (health == Shooter) continue;
            if (health.gameObject == gameObject) continue;

            if (hit.rigidbody)
            {
                hit.rigidbody.velocity += direction * hitForce;
            }

            if (health)
            {
                if (damage > 0.001f)
                {
                    health.Damage(args);
                }
            }

            if (impactFX)
            {
                hitEvent?.Invoke(hit.transform.gameObject, args);
                impactFX.SetActive(true);
                impactFX.transform.SetParent(null);
            }
            Destroy(gameObject);
        }

        age += Time.deltaTime;
        if (age > range / startSpeed)
        {
            if (landFX)
            {
                landEvent?.Invoke();
                landFX.SetActive(true);
                landFX.transform.SetParent(null);
            }
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = Vector3.one * scaleCurve.Evaluate(startSpeed * age / range);
            shadow.Offset = Vector2.down * shadowDistance.Evaluate(startSpeed * age / range);
        }

        previousPosition = rigidbody.position;
    }
}
