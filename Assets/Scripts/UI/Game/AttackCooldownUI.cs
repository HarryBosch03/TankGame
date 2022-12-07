using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the cooldown of a target weapon.
/// </summary>
public class AttackCooldownUI : MonoBehaviour
{
    public GameObject target;
    public RectTransform sliderFill;

    private void Update()
    {
        if (target.TryGetComponent(out IAttack attack))
        {
            Rect containingRect = ((RectTransform)sliderFill.parent).rect;
            sliderFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, containingRect.width * attack.Cooldown);
        }
    }
}
