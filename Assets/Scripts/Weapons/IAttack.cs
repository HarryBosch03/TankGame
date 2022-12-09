using UnityEngine;

/// <summary>
/// Common data for attack based components.
/// </summary>
public interface IAttack
{
    float Cooldown { get; }
    event System.Action AttackEvent;
    event System.Action CooldownFinishedEvent;
}
