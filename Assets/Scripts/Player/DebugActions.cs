using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugActions : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Keyboard.current.numpad7Key.wasPressedThisFrame)
        {
            if (TryGetComponent(out Health health))
            {
                health.Damage(new DamageArgs(null, 0.0f));
            }
        }

        if (Keyboard.current.numpad9Key.wasPressedThisFrame)
        {
            if (TryGetComponent(out Health health))
            {
                health.Die(new DamageArgs(null, float.PositiveInfinity));
            }
        }
    }
#endif
}
