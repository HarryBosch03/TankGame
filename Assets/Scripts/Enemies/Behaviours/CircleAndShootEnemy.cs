using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAndShootEnemy : EnemyBase
{
    public float circleRadius;
    public float attackRadius;
    public float angleOffset;
    public float attackAngle;
    public int circleAttackIndex;

    protected override void Update()
    {
        base.Update();

        if (Target)
        {
            int offset = 1;

            float distToTarget;
            Vector2 direction;
            Vector2 targetPoint;

            do
            {
                Vector2 vectorBetween = (Target.transform.position - transform.position);
                distToTarget = vectorBetween.magnitude;
                direction = vectorBetween / distToTarget;

                if (offset * angleOffset > 360.0f)
                {
                    targetPoint = Target.transform.position;
                    break;
                }

                float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + angleOffset * offset;
                Vector2 offsetDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * circleRadius;

                targetPoint = (Vector2)Target.transform.position + offsetDirection;
                offset++;
            }
            while (Physics2D.OverlapCircle(targetPoint, 1.0f, 1));

            MoveTowards(targetPoint);

            if (distToTarget < attackRadius)
            {
                float dot = Vector2.Dot(direction, MuzzleDirection);
                if (Mathf.Acos(dot) * Mathf.Rad2Deg < attackAngle)
                {
                    Attack(circleAttackIndex);
                }
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.white;
    }
}
