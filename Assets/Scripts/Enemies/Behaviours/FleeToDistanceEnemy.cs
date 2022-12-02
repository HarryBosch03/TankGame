using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeToDistanceEnemy : EnemyBase
{
    public float fleeDistance;
    public float attackDistance;
    public int attackIndex;

    bool fleeing = false;

    protected override void Update()
    {
        base.Update();

        if (Target)
        {
            Vector2 vector = Target.transform.position - transform.position;
            float dist = vector.magnitude;
            Vector2 direction = vector / dist;

            if (fleeing)
            {
                MoveTowards(-direction * 15.0f);

                if (dist > attackDistance)
                {
                    fleeing = false;
                }
            }
            else
            {
                MoveTowards(transform.position);

                Attack(attackIndex);
                if (dist < fleeDistance)
                {
                    fleeing = true;
                }
            }
        }
    }
}
