using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairWeaponIndicator : MonoBehaviour
{
    public GameObject target;
    public GameObject rendererObject;

    private void Update()
    {
        if (target.TryGetComponent(out IAttack attack))
        {
            rendererObject.SetActive(attack.Cooldown > 0.999f);
        }
    }
}
