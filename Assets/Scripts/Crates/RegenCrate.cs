using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenCrate : Crate
{
    public float regenAmount;

    protected override bool TryApplyEffect(GameObject target)
    {
        if (target.TryGetComponent(out Health health))
        {
            health.Regen(new DamageArgs(gameObject, regenAmount));
            return true;
        }
        return false;
    }
}
